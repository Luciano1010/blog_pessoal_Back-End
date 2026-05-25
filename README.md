# Projeto Blog Pessoal Full Stack

Sistema Full Stack de Blog Pessoal desenvolvido com ASP.NET Core no backend e React no frontend.

---

# 🚀 Tecnologias Utilizadas

## Backend

- C#
- ASP.NET Core 7
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- Swagger/OpenAPI
- FluentValidation

## Frontend

- React
- TypeScript
- Vite
- Axios
- React Router DOM

## Deploy

- Render
- Vercel
- GitHub

---

# 📌 Funcionalidades

## Usuários

- Cadastro de usuário
- Login
- Autenticação JWT

## Postagens

- Criar postagens
- Editar postagens
- Deletar postagens
- Listar postagens

## Temas

- Cadastro de temas
- Edição de temas
- Exclusão de temas

## Frontend

- Interface responsiva
- Rotas protegidas
- Integração com API REST
- Gerenciamento de autenticação

---

# 🌐 Aplicação Online

| Serviço | Link |
|---|---|
| Frontend React | https://SEU-FRONT.vercel.app |
| Backend ASP.NET | https://SUA-API.onrender.com |
| Swagger | https://SUA-API.onrender.com/swagger |

---

# 📂 Repositórios

## Backend

https://github.com/Luciano1010/blog_pessoal_Back-End.git

## Frontend

https://github.com/Luciano1010/blogpessoal-frontend.git

---

# 🛠️ Estrutura do Projeto

## Backend

```txt
Controllers
Data
Model
Service
Validator
Configuration
```

## Frontend

```txt
src/
 ├── components
 ├── pages
 ├── services
 ├── models
 ├── routes
 ├── contexts
 └── assets
```

---

# 🔐 Autenticação JWT

A aplicação utiliza autenticação JWT para proteger endpoints privados.

Exemplo de Header:

```http
Authorization: Bearer SEU_TOKEN
```

---

# ⚙️ Configuração Backend

## 1. Clonar repositório

```bash
git clone https://github.com/Luciano1010/blog_pessoal_Back-End.git
```

---

## 2. Entrar no projeto

```bash
cd blog_pessoal_Back-End
```

---

## 3. Restaurar dependências

```bash
dotnet restore
```

---

## 4. Configurar banco PostgreSQL

No arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=db_blogpessoal;Username=postgres;Password=senha"
  }
}
```

---

## 5. Executar migrations

```bash
dotnet ef database update
```

---

## 6. Executar backend

```bash
dotnet run
```

---

# ⚛️ Configuração Frontend

## Entrar na pasta frontend

```bash
cd frontend
```

---

## Instalar dependências

```bash
npm install
```

---

## Executar aplicação React

```bash
npm run dev
```

---

# 🌐 Configuração Axios

```typescript
const api = axios.create({
  baseURL: "https://SUA-API.onrender.com"
})
```

---

# 📚 Swagger

Disponível em:

```txt
https://SUA-API.onrender.com/swagger
```

---

# ☁️ Deploy

## Backend

Deploy realizado no Render.

## Frontend

Hospedagem:
- Vercel
- Render
- Netlify

---

# 🔧 Variáveis de Ambiente

## Backend

```txt
ConnectionStrings__DefaultConnection
```

---

# 🧪 Principais Endpoints

## Usuários

```http
POST /usuarios/cadastrar
POST /usuarios/logar
```

## Postagens

```http
GET /postagens
POST /postagens
PUT /postagens
DELETE /postagens/{id}
```

## Temas

```http
GET /temas
POST /temas
PUT /temas
DELETE /temas/{id}
```

---

# 👨‍💻 Desenvolvedor

## Luciano Simões

GitHub:
https://github.com/Luciano1010

LinkedIn:
https://linkedin.com/in/luciano-simoes10

---

# 📄 Licença

Projeto desenvolvido para fins educacionais e prática de desenvolvimento Full Stack com ASP.NET Core e React.