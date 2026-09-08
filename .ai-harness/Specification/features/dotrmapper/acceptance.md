# Acceptance Criteria — dotrmapper

> Gerado a partir dos **testes unitários** do repositório em 2026-08-24.

## Comando de validação (escopo)

```bash
dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj
```

## Critérios (= testes existentes)

- [ ] AC-T01 — `tests/DotRMapper.Tests/HooksHarnessGapTests.cs` :: Feature_Hooks_NeedsUnitCoverage
- [ ] AC-T02 — `tests/DotRMapper.Tests/ConvertersHarnessGapTests.cs` :: Feature_Converters_NeedsUnitCoverage
- [ ] AC-T03 — `tests/DotRMapper.Tests/MappingHarnessGapTests.cs` :: Feature_Mapping_NeedsUnitCoverage
- [ ] AC-T04 — `tests/DotRMapper.Tests/ConventionMappingTests.cs` :: Map_ShouldCopyMatchingPropertiesByConvention
- [ ] AC-T05 — `tests/DotRMapper.Tests/ConventionMappingTests.cs` :: MapOnto_ShouldUpdateExistingDestinationInstance
- [ ] AC-T06 — `tests/DotRMapper.Tests/ConventionMappingTests.cs` :: Map_WithNullSource_ShouldReturnDefault
- [ ] AC-T07 — `tests/DotRMapper.Tests/ResolversHarnessGapTests.cs` :: Feature_Resolvers_NeedsUnitCoverage
- [ ] AC-T08 — `tests/DotRMapper.Tests/CollectionMappingTests.cs` :: Map_ShouldMapNestedObjects
- [ ] AC-T09 — `tests/DotRMapper.Tests/CollectionMappingTests.cs` :: Map_ShouldMapArrayToList
- [ ] AC-T10 — `tests/DotRMapper.Tests/CollectionMappingTests.cs` :: MapCollection_ShouldMapEnumerableToList
- [ ] AC-T11 — `tests/DotRMapper.Tests/AdvancedMappingTests.cs` :: ReverseMap_ShouldCreateInverseMapping
- [ ] AC-T12 — `tests/DotRMapper.Tests/AdvancedMappingTests.cs` :: AddProfile_ShouldRegisterProfileMappings
- [ ] AC-T13 — `tests/DotRMapper.Tests/AdvancedMappingTests.cs` :: Constructor_WithProfiles_ShouldRegisterMappings
- [ ] AC-T14 — `tests/DotRMapper.Tests/AdvancedMappingTests.cs` :: AssertConfigurationIsValid_ShouldPassForCompleteMapping
- [ ] AC-T15 — `tests/DotRMapper.Tests/AdvancedMappingTests.cs` :: AssertConfigurationIsValid_ShouldThrowForUnmappedMember
- [ ] AC-T16 — `tests/DotRMapper.Tests/AdvancedMappingTests.cs` :: AssertConfigurationIsValid_ShouldIgnoreIgnoredMembers
- [ ] AC-T17 — `tests/DotRMapper.Tests/AdvancedMappingTests.cs` :: Map_ShouldMapMatchingEnumValues
- [ ] AC-T18 — `tests/DotRMapper.Tests/ConfigurationHarnessGapTests.cs` :: Feature_Configuration_NeedsUnitCoverage
- [ ] AC-T19 — `tests/DotRMapper.Tests/CustomMappingTests.cs` :: ForMember_MapFrom_ShouldUseCustomExpression
- [ ] AC-T20 — `tests/DotRMapper.Tests/CustomMappingTests.cs` :: ForMember_Ignore_ShouldSkipDestinationMember
- [ ] AC-T21 — `tests/DotRMapper.Tests/CustomMappingTests.cs` :: AfterMap_ShouldExecuteCallbackAfterMapping
- [ ] AC-T22 — `tests/DotRMapper.Tests/CustomMappingTests.cs` :: BeforeMap_ShouldExecuteCallbackBeforeMapping
- [ ] AC-T23 — `tests/DotRMapper.Tests/CustomMappingTests.cs` :: MapFrom_ValueResolver_ShouldResolveMemberValue
- [ ] AC-T24 — `tests/DotRMapper.Tests/CustomMappingTests.cs` :: ConvertUsing_ShouldApplyTypeConverter
- [ ] AC-T25 — `tests/DotRMapper.Tests/MappingsHarnessGapTests.cs` :: Feature_Mappings_NeedsUnitCoverage
- [ ] AC-T26 — `tests/DotRMapper.Tests/BasedHarnessGapTests.cs` :: Feature_Based_NeedsUnitCoverage
- [ ] AC-T27 — `tests/DotRMapper.Tests/ValidationHarnessGapTests.cs` :: Feature_Validation_NeedsUnitCoverage

## Evidências

| AC | Teste | Arquivo | Comando | Resultado |
|----|-------|---------|---------|-----------|
| AC-T01 | Feature_Hooks_NeedsUnitCoverage | `tests/DotRMapper.Tests/HooksHarnessGapTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~Feature_Hooks_NeedsUnitCoverage` | pending |
| AC-T02 | Feature_Converters_NeedsUnitCoverage | `tests/DotRMapper.Tests/ConvertersHarnessGapTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~Feature_Converters_NeedsUnitCoverage` | pending |
| AC-T03 | Feature_Mapping_NeedsUnitCoverage | `tests/DotRMapper.Tests/MappingHarnessGapTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~Feature_Mapping_NeedsUnitCoverage` | pending |
| AC-T04 | Map_ShouldCopyMatchingPropertiesByConvention | `tests/DotRMapper.Tests/ConventionMappingTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~Map_ShouldCopyMatchingPropertiesByConvention` | pending |
| AC-T05 | MapOnto_ShouldUpdateExistingDestinationInstance | `tests/DotRMapper.Tests/ConventionMappingTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~MapOnto_ShouldUpdateExistingDestinationInstance` | pending |
| AC-T06 | Map_WithNullSource_ShouldReturnDefault | `tests/DotRMapper.Tests/ConventionMappingTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~Map_WithNullSource_ShouldReturnDefault` | pending |
| AC-T07 | Feature_Resolvers_NeedsUnitCoverage | `tests/DotRMapper.Tests/ResolversHarnessGapTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~Feature_Resolvers_NeedsUnitCoverage` | pending |
| AC-T08 | Map_ShouldMapNestedObjects | `tests/DotRMapper.Tests/CollectionMappingTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~Map_ShouldMapNestedObjects` | pending |
| AC-T09 | Map_ShouldMapArrayToList | `tests/DotRMapper.Tests/CollectionMappingTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~Map_ShouldMapArrayToList` | pending |
| AC-T10 | MapCollection_ShouldMapEnumerableToList | `tests/DotRMapper.Tests/CollectionMappingTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~MapCollection_ShouldMapEnumerableToList` | pending |
| AC-T11 | ReverseMap_ShouldCreateInverseMapping | `tests/DotRMapper.Tests/AdvancedMappingTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~ReverseMap_ShouldCreateInverseMapping` | pending |
| AC-T12 | AddProfile_ShouldRegisterProfileMappings | `tests/DotRMapper.Tests/AdvancedMappingTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~AddProfile_ShouldRegisterProfileMappings` | pending |
| AC-T13 | Constructor_WithProfiles_ShouldRegisterMappings | `tests/DotRMapper.Tests/AdvancedMappingTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~Constructor_WithProfiles_ShouldRegisterMappings` | pending |
| AC-T14 | AssertConfigurationIsValid_ShouldPassForCompleteMapping | `tests/DotRMapper.Tests/AdvancedMappingTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~AssertConfigurationIsValid_ShouldPassForCompleteMapping` | pending |
| AC-T15 | AssertConfigurationIsValid_ShouldThrowForUnmappedMember | `tests/DotRMapper.Tests/AdvancedMappingTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~AssertConfigurationIsValid_ShouldThrowForUnmappedMember` | pending |
| AC-T16 | AssertConfigurationIsValid_ShouldIgnoreIgnoredMembers | `tests/DotRMapper.Tests/AdvancedMappingTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~AssertConfigurationIsValid_ShouldIgnoreIgnoredMembers` | pending |
| AC-T17 | Map_ShouldMapMatchingEnumValues | `tests/DotRMapper.Tests/AdvancedMappingTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~Map_ShouldMapMatchingEnumValues` | pending |
| AC-T18 | Feature_Configuration_NeedsUnitCoverage | `tests/DotRMapper.Tests/ConfigurationHarnessGapTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~Feature_Configuration_NeedsUnitCoverage` | pending |
| AC-T19 | ForMember_MapFrom_ShouldUseCustomExpression | `tests/DotRMapper.Tests/CustomMappingTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~ForMember_MapFrom_ShouldUseCustomExpression` | pending |
| AC-T20 | ForMember_Ignore_ShouldSkipDestinationMember | `tests/DotRMapper.Tests/CustomMappingTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~ForMember_Ignore_ShouldSkipDestinationMember` | pending |
| AC-T21 | AfterMap_ShouldExecuteCallbackAfterMapping | `tests/DotRMapper.Tests/CustomMappingTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~AfterMap_ShouldExecuteCallbackAfterMapping` | pending |
| AC-T22 | BeforeMap_ShouldExecuteCallbackBeforeMapping | `tests/DotRMapper.Tests/CustomMappingTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~BeforeMap_ShouldExecuteCallbackBeforeMapping` | pending |
| AC-T23 | MapFrom_ValueResolver_ShouldResolveMemberValue | `tests/DotRMapper.Tests/CustomMappingTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~MapFrom_ValueResolver_ShouldResolveMemberValue` | pending |
| AC-T24 | ConvertUsing_ShouldApplyTypeConverter | `tests/DotRMapper.Tests/CustomMappingTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~ConvertUsing_ShouldApplyTypeConverter` | pending |
| AC-T25 | Feature_Mappings_NeedsUnitCoverage | `tests/DotRMapper.Tests/MappingsHarnessGapTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~Feature_Mappings_NeedsUnitCoverage` | pending |
| AC-T26 | Feature_Based_NeedsUnitCoverage | `tests/DotRMapper.Tests/BasedHarnessGapTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~Feature_Based_NeedsUnitCoverage` | pending |
| AC-T27 | Feature_Validation_NeedsUnitCoverage | `tests/DotRMapper.Tests/ValidationHarnessGapTests.cs` | `dotnet test tests/DotRMapper.Tests/DotRMapper.Tests.csproj --filter FullyQualifiedName~Feature_Validation_NeedsUnitCoverage` | pending |

## Critérios de processo

- [ ] AC-P01 — Use cases em `use-cases.md` coerentes com os testes acima
- [ ] AC-P02 — Sem violação de Knowledge / Governance
- [ ] AC-P03 — Novos comportamentos exigem novo teste antes de marcar DONE
