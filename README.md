# BarberRezende — API de Gestão de Barbearia

API REST desenvolvida em **ASP.NET Core** com **Entity Framework Core**, focada na gestão completa de uma barbearia, permitindo o controle de **clientes**, **barbeiros**, **serviços**, **funcionários** e **agendamentos**.

Este projeto foi criado com o objetivo de aplicar boas práticas de **Clean Architecture**, **SOLID**, separação de responsabilidades e regras de negócio bem definidas, simulando um cenário real de aplicação corporativa.

---

## 🎯 Objetivo do Projeto

O objetivo principal deste projeto é:

- Consolidar conhecimentos em **Back-end .NET**
- Aplicar **arquitetura limpa (clean architecture)** em um sistema real
- Desenvolver uma **API REST profissional**, pronta para ser consumida por qualquer front-end
- Criar uma base sólida para evolução futura (autenticação, front-end, integrações externas)

---

## ✅ Funcionalidades Implementadas

- CRUD completo de:
  - Clientes
  - Barbeiros
  - Serviços
  - Funcionários
  - Agendamentos
- Regras de negócio centralizadas no **Domain**
  - ❌ Não permite dois agendamentos no mesmo horário para o mesmo barbeiro
- Filtros avançados de agendamentos
- Validações de dados
- Documentação interativa com **Swagger**
- Banco de dados SQL Server com **migrations**
- Testes manuais completos via Swagger

---

## 🧱 Arquitetura Utilizada — Clean Architecture

O projeto segue os princípios da **Clean Architecture**, separando responsabilidades por camadas:

### 🔹 Domain
Camada central do sistema (o coração da aplicação).

Responsável por:
- Entidades
- Regras de negócio
- Interfaces de repositórios

> O Domain **não depende de nenhuma outra camada**.

---

### 🔹 Application
Camada responsável pela **orquestração da aplicação**.

Responsável por:
- DTOs (Data Transfer Objects)
- Services (casos de uso)
- Validações
- Mapeamentos (AutoMapper)

> Aqui fica a lógica de aplicação, mas **não regras de negócio puras**.

---

### 🔹 Infrastructure
Camada de infraestrutura e acesso a dados.

Responsável por:
- Entity Framework Core
- DbContext
- Repositórios
- Migrations
- Comunicação com o banco de dados

---

### 🔹 API
Camada de entrada da aplicação.

Responsável por:
- Controllers (endpoints HTTP)
- Configuração do app
- Swagger
- Middlewares
- Injeção de dependência

> A API apenas **recebe a requisição, valida e delega** para a Application.

---

## 🔄 Padrão REST e Métodos HTTP

A API segue o padrão **REST**, utilizando os métodos HTTP:

- `GET` → Buscar dados
- `POST` → Criar registros
- `PUT` → Atualizar registros
- `DELETE` → Remover registros

Todos os endpoints seguem convenções REST e retornam códigos HTTP apropriados.

---

## 🚀 Como Executar o Projeto Localmente

### Pré-requisitos
- .NET SDK (net9.0)
- SQL Server (LocalDB ou instância SQL Server)
- (Opcional) SQL Server Management Studio

---

### 1️⃣ Clonar o repositório
```bash
git clone https://github.com/arthursilvaa-dev/BarberRezende.git
cd BarberRezende
