# 💰 CashFlow

Um projeto pessoal para estudar e aplicar conceitos modernos de desenvolvimento de software, com foco em organização de despesas pessoais.

---

## ✨ Sobre o Projeto

O **CashFlow** é uma **API REST** desenvolvida com **.NET C#**, cujo objetivo é permitir o registro e gerenciamento de despesas financeiras pessoais.  

Este projeto foi idealizado como uma ferramenta de aprendizado contínuo, servindo de laboratório para praticar:

- **Domain-Driven Design (DDD)**: Estrutura modular que facilita o entendimento e a manutenção do domínio da aplicação.
- **Testes de Unidade**: Testes abrangentes com FluentAssertions para garantir a funcionalidade e a qualidade.
- **Geração de Relatórios**: Capacidade de exportar relatórios detalhados para **PDF e Excel**, oferecendo uma análise visual e eficaz das despesas.
- **RESTful API com Documentação Swagger**: Interface documentada que facilita a integração e o teste por parte dos desenvolvedores.

---

## 🧠 Tecnologias e Conceitos Utilizados

- ✅ .NET 9 com C#  
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
│   ├── CashFlow.Api/             → Camada de apresentação (controllers, endpoints, Program.cs)
│   ├── CashFlow.Application/     → Casos de uso e regras de aplicação
│   ├── CashFlow.Communication/   → DTOs e contratos de entrada/saída
│   ├── CashFlow.Domain/          → Entidades, interfaces, enums, regras de negócio
│   ├── CashFlow.Infrastructure/  → Implementações, banco de dados, repositórios
│   ├── CashFlow.Exception/       → Tratamento centralizado de exceções customizadas
│
├── tests/
│   └── CashFlow.Tests/           → Testes unitários com xUnit
│
├── README.md
├── .gitignore
├── LICENSE
└── CashFlow.sln
```

---

## ⚙️ Como Executar Localmente

1.Clone o repositório:
```bash
git clone https://github.com/caiowmp/cashflow.git
```

2.Configure o banco de dados MySQL e atualize a `connection string` em **appsettings.json**.  
3.Restaure os pacotes e execute a aplicação:

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
