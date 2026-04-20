# 🥗 Controle de Dieta (Diet Control API)

O **Controle de Dieta** é uma aplicação robusta desenvolvida
desenvolvida em **C# / .NET** projetada para ajudar utilizadores a monitorizar a sua ingestão calórica de forma inteligente. O diferencial do projeto é a integração com Inteligência Artificial para processar descrições de refeições em linguagem natural, transformando textos simples em dados nutricionais precisos..

---

## 🚀 Funcionalidades

* **Processamento por IA: (Em Andamento)** Integração com a API do Gemini para interpretar descrições de refeições (ex: "Comi dois ovos cozidos e uma banana").
* **Cálculo Metabólico:** Lógica integrada para cálculo de Taxa Metabólica Basal (TMB) e gasto energético.
* **Gestão de Dieta:** Registo e acompanhamento diário de calorias e macronutrientes.
* **Arquitetura Limpa:** Uso de DTOs para transferência de dados segura e organizada.
* **Persistência Portátil:** Utilização de PostgreSQL via Docker para manter o ambiente de desenvolvimento limpo e isolado.

---

## 🛠️ Tecnologias Utilizadas

* **Linguagem:** C#
* **Framework:** .NET (Web API)
* **Banco de Dados:** PostgreSQL
* **Containerização:** Docker & Docker Compose (Em andamento)
* **Ferramentas de Desenvolvimento:** Postman e Swagger(para testes de API e depuração).

---

## 💻 Ambiente de Desenvolvimento

O projeto foi estruturado para ser altamente portátil. Toda a infraestrutura de dados é gerida via containers, garantindo que a aplicação funcione de forma consistente independentemente da máquina local. O desenvolvimento foi focado em performance e tipagem forte para garantir a integridade dos cálculos metabólicos.
