# Use Cases — resolvers

> Derivados de testes/código em 2026-08-24.

## UC-01 — Comportamento principal de `resolvers`

- **Actor:** usuário / consumidor da API
- **Given** o módulo `resolvers` está disponível
- **When** a operação principal descrita em requirements é executada
- **Then** Plug in `IValueResolver` implementations

## UC-02 — Lacuna de teste

- **Actor:** engenharia
- **Given** não há testes unitários mapeados para `resolvers`
- **When** a feature evolui
- **Then** um teste automatizado é adicionado e o acceptance é regenerado
