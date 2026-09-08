# Use Cases — dotrmapper

> Derivados de testes/código em 2026-08-24.

## UC-01 — Map_ShouldCopyMatchingPropertiesByConvention

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMapper.Tests/ConventionMappingTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Map
- **Then** should Copy Matching Properties By Convention

## UC-02 — MapOnto_ShouldUpdateExistingDestinationInstance

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMapper.Tests/ConventionMappingTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Map Onto
- **Then** should Update Existing Destination Instance

## UC-03 — Map_WithNullSource_ShouldReturnDefault

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMapper.Tests/ConventionMappingTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Map With Null Source
- **Then** should Return Default

## UC-04 — Map_ShouldMapNestedObjects

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMapper.Tests/CollectionMappingTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Map
- **Then** should Map Nested Objects

## UC-05 — Map_ShouldMapArrayToList

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMapper.Tests/CollectionMappingTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Map
- **Then** should Map Array To List

## UC-06 — MapCollection_ShouldMapEnumerableToList

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMapper.Tests/CollectionMappingTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Map Collection
- **Then** should Map Enumerable To List

## UC-07 — ReverseMap_ShouldCreateInverseMapping

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMapper.Tests/AdvancedMappingTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Reverse Map
- **Then** should Create Inverse Mapping

## UC-08 — AddProfile_ShouldRegisterProfileMappings

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMapper.Tests/AdvancedMappingTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Add Profile
- **Then** should Register Profile Mappings

## UC-09 — Constructor_WithProfiles_ShouldRegisterMappings

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMapper.Tests/AdvancedMappingTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Constructor With Profiles
- **Then** should Register Mappings

## UC-10 — AssertConfigurationIsValid_ShouldPassForCompleteMapping

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMapper.Tests/AdvancedMappingTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Assert Configuration Is Valid
- **Then** should Pass For Complete Mapping

## UC-11 — AssertConfigurationIsValid_ShouldThrowForUnmappedMember

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMapper.Tests/AdvancedMappingTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Assert Configuration Is Valid
- **Then** Assert Configuration Is Valid Should Throw For Unmapped Member

## UC-12 — AssertConfigurationIsValid_ShouldIgnoreIgnoredMembers

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMapper.Tests/AdvancedMappingTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Assert Configuration Is Valid
- **Then** should Ignore Ignored Members
