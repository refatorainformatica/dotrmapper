# Domain — dotrmapper

> Glossário enriquecido em 2026-08-24 a partir de features + código.

## Glossário

| Termo | Significado | Origem |
|-------|-------------|--------|
| Harness | Estrutura `.ai-harness/` deste repositório | `—` |
| Feature | Unidade em `Specification/features/<id>/` | `—` |
| Run | Execução do agente em `Runtime/state/runs/` | `—` |
| Acceptance | Critérios derivados de testes unitários (AC-T*) | `—` |
| dotrmapper | Projeto DotRMapper | `src/DotRMapper` |
| based | Automatically maps properties with matching names (case-insensitive) | `src/based` |
| mapping | Recursively maps complex object graphs | `src/mapping` |
| configuration | Customize individual members with `ForMember` | `src/configuration` |
| mappings | Generate inverse mappings with `ReverseMap()` | `src/mappings` |
| resolvers | Plug in `IValueResolver` implementations | `src/resolvers` |
| converters | Transform values with `ITypeConverter` | `src/converters` |
| hooks | Run callbacks before or after mapping | `src/hooks` |
| validation | Detect unmapped members with `AssertConfigurationIsValid()` | `src/validation` |
| MapperConfiguration | class em `src/DotRMapper/MapperConfiguration.cs` | `src/DotRMapper/MapperConfiguration.cs` |
| using | class em `src/DotRMapper/MapperConfiguration.cs` | `src/DotRMapper/MapperConfiguration.cs` |
| Mapper | class em `src/DotRMapper/Mapper.cs` | `src/DotRMapper/Mapper.cs` |
| PropertyMapping | class em `src/DotRMapper/Internal/TypeMapConfiguration.cs` | `src/DotRMapper/Internal/TypeMapConfiguration.cs` |
| TypeMapConfiguration | class em `src/DotRMapper/Internal/TypeMapConfiguration.cs` | `src/DotRMapper/Internal/TypeMapConfiguration.cs` |
| MemberMappingKind | enum em `src/DotRMapper/Internal/TypeMapConfiguration.cs` | `src/DotRMapper/Internal/TypeMapConfiguration.cs` |
| MappingEngine | class em `src/DotRMapper/Internal/MappingEngine.cs` | `src/DotRMapper/Internal/MappingEngine.cs` |
| ObjectFactory | class em `src/DotRMapper/Internal/ObjectFactory.cs` | `src/DotRMapper/Internal/ObjectFactory.cs` |
| TypeMapRegistry | class em `src/DotRMapper/Internal/TypeMapRegistry.cs` | `src/DotRMapper/Internal/TypeMapRegistry.cs` |
| ExpressionHelper | class em `src/DotRMapper/Internal/ExpressionHelper.cs` | `src/DotRMapper/Internal/ExpressionHelper.cs` |
| ParameterReplacer | class em `src/DotRMapper/Internal/ExpressionHelper.cs` | `src/DotRMapper/Internal/ExpressionHelper.cs` |
| DotRMapperConfigurationException | class em `src/DotRMapper/Exceptions/DotRMapperConfigurationException.cs` | `src/DotRMapper/Exceptions/DotRMapperConfigurationException.cs` |
| ResolutionContext | class em `src/DotRMapper/Abstractions/ResolutionContext.cs` | `src/DotRMapper/Abstractions/ResolutionContext.cs` |
| MapperConfigurationExpression | class em `src/DotRMapper/Configuration/MapperConfigurationExpression.cs` | `src/DotRMapper/Configuration/MapperConfigurationExpression.cs` |
| MappingExpression | class em `src/DotRMapper/Configuration/MappingExpression.cs` | `src/DotRMapper/Configuration/MappingExpression.cs` |
| MemberConfigurationExpression | class em `src/DotRMapper/Configuration/MemberConfigurationExpression.cs` | `src/DotRMapper/Configuration/MemberConfigurationExpression.cs` |
| for | class em `src/DotRMapper/Abstractions/Configuration/Profile.cs` | `src/DotRMapper/Abstractions/Configuration/Profile.cs` |
| Profile | class em `src/DotRMapper/Abstractions/Configuration/Profile.cs` | `src/DotRMapper/Abstractions/Configuration/Profile.cs` |
| ValueResolver | interface em `src/DotRMapper/Abstractions/Resolvers/IValueResolver.cs` | `src/DotRMapper/Abstractions/Resolvers/IValueResolver.cs` |
| TypeConverter | interface em `src/DotRMapper/Abstractions/Converters/ITypeConverter.cs` | `src/DotRMapper/Abstractions/Converters/ITypeConverter.cs` |

## Como manter

1. Novo conceito de domínio → linha neste glossário **na mesma PR**.
2. Mesmo identificador em SPEC, código e testes.
3. ADR se o termo implica trade-off arquitetural.
