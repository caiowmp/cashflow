# 💰 CashFlow

Um projeto pessoal para estudar e aplicar conceitos modernos de desenvolvimento de software, com foco em organização de despesas pessoais.

---

## ✨ Sobre o Projeto

O **CashFlow** é uma **API REST** desenvolvida com **.NET C#**, cujo objetivo é permitir o registro e gerenciamento de despesas financeiras pessoais.  

Este projeto foi idealizado como uma ferramenta de aprendizado contínuo, servindo de laboratório para praticar:

- Boas práticas de arquitetura de software (como **DDD** e **SOLID**)  
- Integração com banco de dados  
- Testes automatizados  
- Design limpo e escalável  

---

## 🧠 Tecnologias e Conceitos Utilizados

- ✅ .NET 8 com C#  
- ✅ API REST  
- ✅ MySQL como banco de dados relacional  
- ✅ DDD (Domain-Driven Design)  
- ✅ SOLID principles  
- ✅ Testes unitários com xUnit  
- ✅ Camadas bem definidas (**Domain, Application, Infrastructure, API**)  
- ✅ Injeção de dependência  
- 🔜 Mais conceitos e ferramentas a serem explorados em breve  

---

## 📂 Estrutura do Projeto

```
CashFlow/
│
├── src/
│   ├── CashFlow.API/             → Camada de apresentação (controllers, endpoints)
│   ├── CashFlow.Application/     → Casos de uso e regras de aplicação
│   ├── CashFlow.Domain/          → Entidades, interfaces, enums, regras de negócio
│   ├── CashFlow.Infrastructure/  → Implementações, banco de dados, repositórios
│
├── tests/
│   └── CashFlow.Tests/           → Testes unitários com xUnit
│
├── README.md
└── CashFlow.sln
```

---

## ⚙️ Como Executar Localmente

Clone o repositório:

```bash
git clone https://github.com/caiowmp/cashflow.git
cd cashflow
```

Configure o banco de dados MySQL e atualize a `connection string` em **appsettings.json**.  

Restaure os pacotes e execute a aplicação:

```bash
dotnet restore
dotnet run --project src/CashFlow.API
```

Para rodar os testes:

```bash
dotnet test
```

---

## 🧪 Testes

Os testes estão sendo desenvolvidos com o framework **xUnit**.  
Eles cobrem principalmente as regras de negócio e os casos de uso.
