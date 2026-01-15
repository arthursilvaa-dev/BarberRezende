\# BarberRezende



API para gerenciamento de barbearia: clientes, barbeiros, funcionários, serviços e agendamentos.



\## Tecnologias

\- .NET 9

\- ASP.NET Core Web API

\- Entity Framework Core (SQL Server)

\- Swagger (Swashbuckle)

\- Clean Architecture (Domain / Application / Infrastructure / API)



\## Como rodar (desenvolvimento)

1\. Clone o projeto

2\. Configure a connection string em `BarberRezende.API/appsettings.Development.json`

3\. Rode as migrations (se usar):

&nbsp;  - `dotnet ef database update --project BarberRezende.Infrastructure --startup-project BarberRezende.API`

4\. Execute a API:

&nbsp;  - `dotnet run --project BarberRezende.API`



\## Swagger

Após rodar, abra:

\- `http://localhost:5284/swagger`



\## Estrutura

\- \*\*Domain\*\*: entidades e contratos (regras do negócio)

\- \*\*Application\*\*: DTOs, interfaces de serviços, mapeamentos e lógica de aplicação

\- \*\*Infrastructure\*\*: EF Core, DbContext, repositories e migrations

\- \*\*API\*\*: controllers, injeção de dependência, swagger e endpoints



