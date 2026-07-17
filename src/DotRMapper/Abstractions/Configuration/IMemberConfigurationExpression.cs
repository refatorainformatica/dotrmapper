using System.Linq.Expressions;
using DotRMapper.Abstractions.Converters;
using DotRMapper.Abstractions.Resolvers;

namespace DotRMapper.Abstractions.Configuration;

/// <summary>
/// Defines the contract for configuring an individual destination member mapping.
/// </summary>
/// <remarks>
/// Configured inside <see cref="IMappingExpression{TSource, TDestination}.ForMember{TMember}"/>.
/// Implemented by the internal member configuration expression type.
/// </remarks>
/// <typeparam name="TSource">The source type.</typeparam>
/// <typeparam name="TDestination">The destination type.</typeparam>
/// <typeparam name="TMember">The destination member type.</typeparam>
public interface IMemberConfigurationExpression<TSource, TDestination, TMember>
{
    /// <summary>
    /// Maps the destination member from the specified source member expression.
    /// </summary>
    /// <remarks>
    /// Compiles the expression into a source accessor at configuration time.
    /// </remarks>
    /// <param name="sourceMember">An expression identifying the source value.</param>
    void MapFrom<TSourceMember>(Expression<Func<TSource, TSourceMember>> sourceMember);

    /// <summary>
    /// Maps the destination member using a custom resolver function with access to the destination instance.
    /// </summary>
    /// <remarks>
    /// Invoked once per mapped instance during member resolution.
    /// </remarks>
    /// <param name="resolver">The resolver function.</param>
    void MapFrom(Func<TSource, TDestination, TMember> resolver);

    /// <summary>
    /// Maps the destination member using a custom resolver function with access to the resolution context.
    /// </summary>
    /// <remarks>
    /// Provides full mapping context including the root <see cref="ResolutionContext.Mapper"/> instance.
    /// </remarks>
    /// <param name="resolver">The resolver function.</param>
    void MapFrom(Func<TSource, TDestination, TMember, ResolutionContext, TMember> resolver);

    /// <summary>
    /// Maps the destination member using a registered value resolver type.
    /// </summary>
    /// <remarks>
    /// <typeparamref name="TValueResolver"/> must implement
    /// <see cref="IValueResolver{TSource, TDestination, TDestMember}"/> and expose a parameterless constructor.
    /// </remarks>
    /// <typeparam name="TValueResolver">The value resolver type.</typeparam>
    void MapFrom<TValueResolver>()
        where TValueResolver : IValueResolver<TSource, TDestination, TMember>, new();

    /// <summary>
    /// Ignores the destination member during mapping.
    /// </summary>
    /// <remarks>
    /// Excluded from validation when <see cref="MapperConfiguration.AssertConfigurationIsValid"/> runs.
    /// </remarks>
    void Ignore();

    /// <summary>
    /// Uses a type converter to convert the source value to the destination member type.
    /// </summary>
    /// <remarks>
    /// Applies to the convention-matched source property value when one exists.
    /// </remarks>
    /// <typeparam name="TConverter">The type converter.</typeparam>
    void ConvertUsing<TConverter>()
        where TConverter : ITypeConverter, new();
}
