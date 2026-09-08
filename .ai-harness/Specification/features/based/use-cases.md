# Use Cases — based

> Derivados de testes/código em 2026-08-24.

## UC-01 — Comportamento principal de `based`

- **Actor:** usuário / consumidor da API
- **Given** o módulo `based` está disponível
- **When** a operação principal descrita em requirements é executada
- **Then** Automatically maps properties with matching names (case-insensitive)

## UC-02 — Lacuna de teste

- **Actor:** engenharia
- **Given** não há testes unitários mapeados para `based`
- **When** a feature evolui
- **Then** um teste automatizado é adicionado e o acceptance é regenerado
