# Use Cases — mappings

> Derivados de testes/código em 2026-08-24.

## UC-01 — Comportamento principal de `mappings`

- **Actor:** usuário / consumidor da API
- **Given** o módulo `mappings` está disponível
- **When** a operação principal descrita em requirements é executada
- **Then** Generate inverse mappings with `ReverseMap()`

## UC-02 — Lacuna de teste

- **Actor:** engenharia
- **Given** não há testes unitários mapeados para `mappings`
- **When** a feature evolui
- **Then** um teste automatizado é adicionado e o acceptance é regenerado
