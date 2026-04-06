# 🦁 BarberRezende — Solução Corporativa de Gestão de Barbearia
Sistema Full-Stack de alto nível desenvolvido com o ecossistema .NET 9, composto por uma API REST robusta e uma aplicação Web MVC dinâmica. O projeto foi concebido sob rigorosos padrões de engenharia para resolver desafios complexos de agendamento e gestão financeira em cenários reais.

## 🛠️ Stack Tecnológica & Diferenciais Técnicos
Linguagem & Framework: C# com .NET 9 (LTS).

Arquitetura: Clean Architecture com separação rigorosa de responsabilidades em camadas (Domain, Application, Infrastructure, API, Web).

Persistência: Entity Framework Core com SQL Server, utilizando Fluent API para mapeamentos granulares e snapshots de dados.

Segurança: Autenticação e Autorização baseadas em JWT (JSON Web Token).

Boas Práticas: Aplicação de princípios SOLID, DRY, Async/Await para alta escalabilidade e AutoMapper para transformação segura de objetos.

Resiliência: Middleware customizado para tratamento global de exceções padronizado com ProblemDetails.

### 🌟 Funcionalidades de Negócio
Dashboard Administrativo: Painel dinâmico com métricas de faturamento mensal (atual vs histórico) e indicadores operacionais em tempo real.

Agendamento Premium (UX): Interface moderna inspirada no Booksy, com calendário visual e seleção dinâmica de horários baseada na duração real de cada serviço (45min, 30min, 1h, etc.).

Regras de Domínio: Motor de validação para prevenção de conflitos de agenda por profissional e garantia de integridade financeira via Snapshots.

Gestão de Equipe e Catálogo: CRUDs completos com filtros dinâmicos para Barbeiros, Clientes e Serviços.

#### 📖 Como Executar o Projeto
Pré-requisitos:
.NET SDK 9.0

SQL Server (LocalDB ou superior)

Clonagem e Configuração:

Bash
git clone https://github.com/arthursilvaa-dev/BarberRezende.git
Banco de Dados:
Ajuste a DefaultConnection no appsettings.json da API e execute o comando abaixo na raiz:

Bash
dotnet ef database update --project BarberRezende.Infrastructure --startup-project BarberRezende.API
Execução: Inicie os projetos BarberRezende.API e BarberRezende.Web simultaneamente.

##### Credenciais Padrão (Seed):
E-mail: admin@barberrezende.com

Senha: 123456

Desenvolvido por Arthur Silva como projeto de portfólio focado em Engenharia de Software e Melhores Práticas .NET.
