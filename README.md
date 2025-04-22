# ASMED

Sistema voltado para médicos, com foco em facilitar a consulta e gestão de informações clínicas, como:
- Consultas agendadas
- Dados de pacientes
- Cirurgias realizadas
- Detalhes específicos de cada procedimento

## 🧱 Arquitetura

Projeto baseado em .NET 6 (API) + Angular (SPA), utilizando padrões e boas práticas:
- ASP.NET Core 6 + EF Core (Code First)
- ASP.NET Identity com autenticação via JWT
- Angular 17 com Angular Material
- Separação em camadas: API, Application, Domain, Data, Core, Shared
- Unit of Work, Repository Pattern, Service Layer
- Logs com Serilog
- Testes com xUnit, Moq e FluentAssertions

## 🗂 Estrutura da Solution
ASMED/ ├── ASMED.API/ → API ASP.NET Core ├── ASMED.Application/ → Regras de negócio (serviços, DTOs) ├── ASMED.Domain/ → Entidades e lógica de domínio ├── ASMED.Data/ → EF Core, DbContext, repositórios ├── ASMED.Core/ → Abstrações, helpers, exceptions ├── ASMED.Shared/ → Enums, mensagens, validações comuns ├── ASMED.Tests/ → Testes automatizados (xUnit) └── ASMED.Web/ → Projeto Angular (frontend SPA)

## 🚀 Como rodar localmente

1. **Requisitos**:
   - .NET 6 SDK
   - Node.js 18+ (para frontend)
   - SQL Server
   - Angular CLI

2. **Clone o projeto**:
   ```bash
   git clone https://github.com/DiLeite/ASMED-Pipefy.git
   cd ASMED-Pipefy
