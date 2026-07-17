using System.Linq.Expressions;
using System.Reflection;

namespace DotRMapper.Internal;

/// <summary>
/// Provides helpers for working with member access expressions during configuration.
/// </summary>
/// <remarks>
/// Used when resolving destination properties and compiling source member accessors.
/// </remarks>
internal static class ExpressionHelper
{
    /// <summary>
    /// Extracts the <see cref="PropertyInfo"/> referenced by a member access expression.
    /// </summary>
    /// <remarks>
    /// Supports direct property access and unary conversion wrappers. Throws when the expression
    /// does not refer to a property.
    /// </remarks>
    /// <typeparam name="T">The type containing the property.</typeparam>
    /// <typeparam name="TMember">The property value type.</typeparam>
    /// <param name="expression">The member access expression.</param>
    /// <returns>The referenced property metadata.</returns>
    public static PropertyInfo GetPropertyInfo<T, TMember>(Expression<Func<T, TMember>> expression)
    {
        if (
            expression.Body is MemberExpression memberExpression
            && memberExpression.Member is PropertyInfo propertyInfo
        )
        {
            return propertyInfo;
        }

        if (
            expression.Body is UnaryExpression { Operand: MemberExpression unaryMember }
            && unaryMember.Member is PropertyInfo convertedProperty
        )
        {
            return convertedProperty;
        }

        throw new ArgumentException("Expression must be a property accessor.", nameof(expression));
    }

    /// <summary>
    /// Compiles a source member expression into a delegate that returns an object value.
    /// </summary>
    /// <remarks>
    /// Replaces the original parameter with a typed source parameter and boxes the result.
    /// </remarks>
    /// <typeparam name="TSource">The source type.</typeparam>
    /// <param name="expression">The source member lambda expression.</param>
    /// <returns>A compiled accessor delegate.</returns>
    public static Func<TSource, object?> CompileSourceAccessor<TSource>(LambdaExpression expression)
    {
        var sourceParam = Expression.Parameter(typeof(TSource), "source");
        var replaced = new ParameterReplacer(expression.Parameters[0], sourceParam).Visit(
            expression.Body
        )!;
        var converted = Expression.Convert(replaced, typeof(object));
        var lambda = Expression.Lambda<Func<TSource, object?>>(converted, sourceParam);
        return lambda.Compile();
    }

    /// <summary>
    /// Replaces a lambda parameter with a different parameter expression during tree rewriting.
    /// </summary>
    /// <remarks>
    /// Used when rebinding source member expressions to a typed source parameter.
    /// </remarks>
    /// <param name="source">The parameter expression to replace.</param>
    /// <param name="target">The replacement parameter expression.</param>
    private sealed class ParameterReplacer(ParameterExpression source, ParameterExpression target)
        : ExpressionVisitor
    {
        /// <summary>
        /// Replaces matching parameter nodes with the target parameter.
        /// </summary>
        /// <remarks>
        /// Delegates to the base visitor for non-matching parameters.
        /// </remarks>
        /// <param name="node">The parameter expression being visited.</param>
        /// <returns>The rewritten expression node.</returns>
        protected override Expression VisitParameter(ParameterExpression node) =>
            node == source ? target : base.VisitParameter(node);
    }
}
