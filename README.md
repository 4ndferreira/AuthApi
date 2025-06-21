# AuthApi

![.NET Version](https://img.shields.io/badge/.NET-8.0-blue)
![License](https://img.shields.io/badge/license-MIT-green)
![Build](https://img.shields.io/badge/build-passing-brightgreen)

AuthApi é uma API de autenticação e autorização baseada em JWT desenvolvida em .NET 8, criada com o objetivo de praticar padrões modernos de autenticação, boas práticas de arquitetura em camadas e testabilidade.  
Este projeto foi desenvolvido como um modelo de referência para aplicações web e mobile que precisem de um sistema seguro e simples de login.

> ✅ Explore os endpoints diretamente com a documentação interativa via Swagger!

## Sumário

- [Pré-requisitos](#pré-requisitos)
- [Principais características](#principais-características)
- [Como executar](#como-executar)
- [Como gerar uma chave secreta forte para o JWT](#como-gerar-uma-chave-secreta-forte-para-o-jwt)
- [Exemplos de uso dos endpoints](#exemplos-de-uso-dos-endpoints)
- [Rodando os testes](#rodando-os-testes)
- [Contribuição](#contribuição)

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQLite (o banco é criado automaticamente, mas é útil ter uma ferramenta para inspecionar o banco)
- Ferramenta `dotnet-ef` para migrações:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

## Principais características

- Estrutura modular separando domínio, aplicação, infraestrutura, API e testes
- Autenticação e autorização via JWT
- Integração com Entity Framework Core e banco de dados SQLite
- Endpoints para registro, login, refresh de token e perfil do usuário
- Documentação automática via Swagger
- Projeto já inclui estrutura para testes unitários (exemplo: AuthApi.Tests)
- Suporte a execução de testes via `dotnet test`
- Código desacoplado para facilitar o uso de mocks e stubs

## Como executar

1. Restaure as dependências do projeto:

   ```bash
   dotnet restore
   ```

2. Execute as migrações do banco de dados:

   ```bash
   dotnet ef database update --project AuthApi.Infrastructure
   ```

3. Configure o JWT (obrigatório para rodar a aplicação):

   - **User Secrets** (recomendado para desenvolvimento):
     ```bash
     dotnet user-secrets set "Jwt:Key" "sua-chave-secreta" --project AuthApi.Bfi
     dotnet user-secrets set "Jwt:Issuer" "AuthApi" --project AuthApi.Bfi
     dotnet user-secrets set "Jwt:Audience" "AuthApiUser" --project AuthApi.Bfi
     dotnet user-secrets set "Jwt:ExpireMinutes" "60" --project AuthApi.Bfi
     ```

   - **Ou `appsettings.json`**:
     ```json
     "Jwt": {
       "Key": "sua-chave-secreta",
       "Issuer": "AuthApi",
       "Audience": "AuthApiUser",
       "ExpireMinutes": 60
     }
     ```

   - **Ou variáveis de ambiente**:
     ```bash
     export Jwt__Key=sua-chave-secreta
     export Jwt__Issuer=AuthApi
     export Jwt__Audience=AuthApiUser
     export Jwt__ExpireMinutes=60
     ```

## Como gerar uma chave secreta forte para o JWT

Para garantir a segurança do seu JWT, utilize uma chave secreta forte e aleatória. Veja como gerar uma chave segura usando o terminal ou PowerShell:

- **No Linux/macOS (Terminal):**
  ```bash
  openssl rand -base64 32
  ```

- **No Windows (PowerShell):**
  ```powershell
  $bytes = New-Object 'System.Byte[]' 32; (New-Object System.Security.Cryptography.RNGCryptoServiceProvider).GetBytes($bytes); [Convert]::ToBase64String($bytes)
  ```

Copie o valor gerado e utilize como valor da configuração `Jwt:Key`.

4. Rode a aplicação:

   ```bash
   dotnet run --project AuthApi.Bfi
   ```

5. Acesse a documentação interativa (Swagger):

   - https://localhost:7252/swagger  
   - http://localhost:5104/swagger

## Exemplos de uso dos endpoints

- **Registro:**  
  `POST /api/auth/register`  
  Corpo (JSON):
  ```json
  {
    "email": "seu@email.com",
    "password": "senha"
  }
  ```

- **Login:**  
  `POST /api/auth/login`  
  Corpo (JSON):
  ```json
  {
    "email": "seu@email.com",
    "password": "senha"
  }
  ```

- **Refresh Token:**  
  `POST /api/auth/refresh`  
  Corpo (JSON):
  ```json
  {
    "refreshToken": "<token>"
  }
  ```

- **Perfil:**  
  `GET /api/user/profile`  
  (necessário incluir o JWT no header `Authorization`)

  Exemplo de uso do header:

  - No Swagger, clique em "Authorize" e insira:  
    `Bearer <seu_token_jwt_aqui>`

  - Em uma requisição HTTP (exemplo usando `curl`):
    ```bash
    curl -H "Authorization: Bearer <seu_token_jwt_aqui>" https://localhost:7252/api/user/profile
    ```

  - No Postman, adicione um header:
    | Key           | Value                       |
    |---------------|-----------------------------|
    | Authorization | Bearer <seu_token_jwt_aqui> |

## Rodando os testes

Execute os testes unitários com:

```bash
dotnet test
```

## Contribuição

Sinta-se à vontade para abrir issues ou enviar pull requests para contribuir com o projeto.

---

> Projeto em desenvolvimento inicial. Sugestões são bem-vindas!
