# Architecture — dotrmapper

> Gerado automaticamente em 2026-08-24. Revise e refine.

## Estilo

- .NET modular

## Estrutura (topo do repositório)

```text
dotrmapper/
├── docs/
│   ├── configuration-guide.md
│   ├── examples.md
│   └── getting-started.md
├── src/
│   └── DotRMapper/
│       ├── Abstractions/
│       ├── Configuration/
│       ├── Converters/
│       ├── Exceptions/
│       ├── Internal/
│       ├── Resolvers/
│       ├── DotRMapper.csproj
│       ├── Mapper.cs
│       └── MapperConfiguration.cs
├── tests/
│   └── DotRMapper.Tests/
│       ├── Models/
│       ├── Profiles/
│       ├── AdvancedMappingTests.cs
│       ├── CollectionMappingTests.cs
│       ├── ConventionMappingTests.cs
│       ├── CustomMappingTests.cs
│       ├── DotRMapper.Tests.csproj
│       └── GlobalUsings.cs
├── banner.jpg
├── DotRMapper.slnx
├── LICENSE
└── README.md
```

## Raiz de código principal

`src`

## Stack detectada

| Camada | Tecnologia |
|--------|------------|
| Runtime | .NET (DotRMapper.slnx) |
| Harness | `.ai-harness/` |

## Relação harness ↔ código

| Spec feature | Código |
|--------------|--------|
| `Specification/features/<id>/` | módulo/pasta correspondente |

## Cross-cutting (preencher)

| Peça | Papel |
|------|--------|
| DI | ver README / código |
| Auth | ver README / código |
| Persistência | ver README / código |
| Observability | ver README / código |

## Fora de escopo arquitetural (atual)

- Consultar README.md e issues abertas
