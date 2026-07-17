using System.Linq.Expressions;
using DotRMapper.Abstractions;
using DotRMapper.Abstractions.Configuration;
using DotRMapper.Abstractions.Converters;
using DotRMapper.Abstractions.Resolvers;
using DotRMapper.Internal;

namespace DotRMapper.Configuration;

/// <summary>
/// Default implementation of <see cref="IMemberConfigurationExpression{TSource, TDestination, TMember}"/>.
/// </summary>
/// <remarks>
/// Mutates a single <see cref="PropertyMapping"/> entry for the configured destination member.
/// </remarks>
/// <typeparam name="TSource">The source type.</typeparam>
/// <typeparam name="TDestination">The destination type.</typeparam>
/// <typeparam name="TMember">The destination member type.</typeparam>
/// <param name="propertyMapping">The property mapping to configure.</param>
internal sealed class MemberConfigurationExpression<TSource, TDestination, TMember>(
    PropertyMapping propertyMapping
) : IMemberConfigurationExpression<TSource, TDestination, TMember>
{
    /// <summary>
    /// Maps the destination member from the specified source member expression.
    /// </summary>
    /// <remarks>
    /// Compiles the expression into a source accessor at configuration time.
    /// </remarks>
    /// <param name="sourceMember">An expression identifying the source value.</param>
    public void MapFrom<TSourceMember>(Expression<Func<TSource, TSourceMember>> sourceMember)
    {
        propertyMapping.Kind = MemberMappingKind.Expression;
        propertyMapping.SourceExpression = sourceMember;
        propertyMapping.CustomResolver = ExpressionHelper.CompileSourceAccessor<TSource>(
            sourceMember
        );
        propertyMapping.IsIgnored = false;
    }

    /// <summary>
    /// Maps the destination member using a custom resolver function with access to the destination instance.
    /// </summary>
    /// <remarks>
    /// Invoked once per mapped instance during member resolution.
    /// </remarks>
    /// <param name="resolver">The resolver function.</param>
    public void MapFrom(Func<TSource, TDestination, TMember> resolver)
    {
        propertyMapping.Kind = MemberMappingKind.FuncWithDestination;
        propertyMapping.CustomResolver = resolver;
        propertyMapping.IsIgnored = false;
    }

    /// <summary>
    /// Maps the destination member using a custom resolver function with access to the resolution context.
    /// </summary>
    /// <remarks>
    /// Provides full mapping context including the root <see cref="ResolutionContext.Mapper"/> instance.
    /// </remarks>
    /// <param name="resolver">The resolver function.</param>
    public void MapFrom(Func<TSource, TDestination, TMember, ResolutionContext, TMember> resolver)
    {
        propertyMapping.Kind = MemberMappingKind.FuncWithContext;
        propertyMapping.CustomResolver = resolver;
        propertyMapping.IsIgnored = false;
    }

    /// <summary>
    /// Maps the destination member using a registered value resolver type.
    /// </summary>
    /// <remarks>
    /// <typeparamref name="TValueResolver"/> must implement
    /// <see cref="IValueResolver{TSource, TDestination, TDestMember}"/> and expose a parameterless constructor.
    /// </remarks>
    /// <typeparam name="TValueResolver">The value resolver type.</typeparam>
    public void MapFrom<TValueResolver>()
        where TValueResolver : IValueResolver<TSource, TDestination, TMember>, new()
    {
        propertyMapping.Kind = MemberMappingKind.ValueResolver;
        propertyMapping.ValueResolverType = typeof(TValueResolver);
        propertyMapping.IsIgnored = false;
    }

    /// <summary>
    /// Ignores the destination member during mapping.
    /// </summary>
    /// <remarks>
    /// Excluded from validation when <see cref="MapperConfiguration.AssertConfigurationIsValid"/> runs.
    /// </remarks>
    public void Ignore()
    {
        propertyMapping.Kind = MemberMappingKind.Ignored;
        propertyMapping.IsIgnored = true;
    }

    /// <summary>
    /// Uses a type converter to convert the source value to the destination member type.
    /// </summary>
    /// <remarks>
    /// Applies to the convention-matched source property value when one exists.
    /// </remarks>
    /// <typeparam name="TConverter">The type converter.</typeparam>
    public void ConvertUsing<TConverter>()
        where TConverter : ITypeConverter, new()
    {
        propertyMapping.Kind = MemberMappingKind.TypeConverter;
        propertyMapping.TypeConverterType = typeof(TConverter);
        propertyMapping.IsIgnored = false;
    }
}
