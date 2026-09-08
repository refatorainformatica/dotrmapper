# Use Cases — validation

> Derivados de testes/código em 2026-08-24.

## UC-01 — Comportamento principal de `validation`

- **Actor:** usuário / consumidor da API
- **Given** o módulo `validation` está disponível
- **When** a operação principal descrita em requirements é executada
- **Then** Detect unmapped members with `AssertConfigurationIsValid()`

## UC-02 — Lacuna de teste

- **Actor:** engenharia
- **Given** não há testes unitários mapeados para `validation`
- **When** a feature evolui
- **Then** um teste automatizado é adicionado e o acceptance é regenerado
