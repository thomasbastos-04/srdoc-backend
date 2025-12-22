# SrDoc - Sistema de Gestão de Documentos (Backend)

## 🎯 Sobre o Projeto
O SrDoc é uma API robusta desenvolvida em **.NET 8** focada na gestão de documentos e signatários. O projeto foi arquitetado seguindo os princípios de **Cloud Native**, com foco em escalabilidade, segurança (Identity) e automação de deploy (CI/CD).

## 🏗️ Arquitetura da Solução
A infraestrutura está hospedada na **Azure**, utilizando os seguintes componentes:
- **Azure Kubernetes Service (AKS):** Orquestrador de containers para hospedagem da API.
- **Azure SQL Database:** Banco de dados relacional para persistência de dados e Identity.
- **Azure Container Registry (ACR):** Repositório privado para armazenamento de imagens Docker.
- **Azure DevOps:** Automação do ciclo de vida da aplicação (Pipelines).

---

## 🛠️ O que foi implementado até agora

### 1. Backend & Segurança
- **ASP.NET Identity:** Implementação de autenticação e autorização via endpoints nativos (`AddIdentityApiEndpoints`).
- **Entity Framework Core:** Modelagem do domínio (Documents, Signatories) e controle de versões via Migrations.
- **Swagger/OpenAPI:** Configurado para documentação e teste dos endpoints.

### 2. DevOps & Containerização
- **Dockerfile Multi-Stage:** Otimizado para reduzir o tamanho da imagem de produção.
- **Azure Pipelines:** Configuração do arquivo `azure-pipelines.yml` para Build, Push (ACR) e Deploy (AKS).
- **Kubernetes Manifests:** - `deployment.yaml`: Configuração de réplicas e injeção de segredos.
  - `service.yaml`: Exposição da API via LoadBalancer com IP público.

### 3. Configurações de Nuvem (Azure)
- **Resource Group:** `srdoc` (Localizado em Brazil South).
- **Container Registry:** `srdocregistry` (SKU Basic).
- **SQL Server:** `sql-server-srdoc` (Autenticação SQL: `srdoc`).
- **AKS Cluster:** `aks-srdoc` (Preset: Dev/Test | Nodes: Standard_B2s).

---

## 🚀 Guia de Configuração e Deploy

### Variáveis de Ambiente e Segredos
As credenciais sensíveis não são armazenadas no código. No cluster AKS, deve-se criar o segredo para a conexão com o banco:

```bash
kubectl create secret generic srdoc-db-secret \
  --from-literal=connection-string="Server=tcp:sql-server-srdoc.database.windows.net,1433;Initial Catalog=srdoc-hml;User ID=srdoc;Password=SUA_SENHA_AQUI;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"