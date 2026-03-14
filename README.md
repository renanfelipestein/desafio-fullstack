## Tecnologias utilizadas

| Camada    | Tecnologia                              |
|-----------|-----------------------------------------|
| Backend   | .NET 8 (ASP.NET Core), Entity Framework Core 8 |
| Banco     | PostgreSQL 16                           |
| Frontend  | Vue 3 + TypeScript + Vite               |
| UI        | Bootstrap 5, Chart.js                  |
| Auth      | JWT Bearer Token                        |
| Clima API | OpenWeatherMap                          |
| Container | Docker + Docker Compose                 |

---

## Pré-requisitos

- [Docker](https://www.docker.com/) instalado
- [Docker Compose](https://docs.docker.com/compose/) instalado

---

## Como executar com Docker

### 1. Clone o repositório

```bash
git clone <url-do-repositorio>
cd desafio-fullstack
```

### 2. Suba os containers

```bash
docker-compose up --build
```

O comando acima irá:
- Criar e iniciar o banco de dados PostgreSQL
- Compilar e iniciar o backend .NET 8 (com migrations automáticas)
- Compilar e servir o frontend Vue 3 via Nginx

### 3. Acesse a aplicação

| Serviço         | URL                          |
|-----------------|------------------------------|
| Frontend        | http://localhost             |
| Backend/Swagger | http://localhost:8080/swagger |
| Health Check    | http://localhost:8080/health  |

### 4. Login padrão

```
Email:  clover@aliare.co
Senha:  clover123
```

### Parar os containers

```bash
docker-compose down
```

### Parar e remover os dados do banco

```bash
docker-compose down -v
```

---

## Como executar localmente (sem Docker)

### Pré-requisitos locais

- .NET 8 SDK
- Node.js 20+
- PostgreSQL rodando em `localhost:5432`

### Backend

```bash
cd backend
dotnet restore
dotnet run
# Disponível em http://localhost:5063
```

### Frontend

```bash
cd frontend
npm install
npm run dev
# Disponível em http://localhost:5173
```

> O arquivo `frontend/.env` já está configurado com `VITE_API_URL=http://localhost:5063` para desenvolvimento local.

---

## Estrutura do projeto

```
desafio-fullstack/
├── backend/
│   ├── Controllers/        # Endpoints da API
│   ├── Services/           # ClimaService (OpenWeatherMap), JwtService
│   ├── Models/             # Entidades e DTOs
│   ├── Data/               # AppDbContext (Entity Framework)
│   ├── Migrations/         # Migrations do banco
│   ├── Dockerfile
│   └── .dockerignore
├── frontend/
│   ├── src/
│   │   └── components/     # Login, ConsultaCidade, ConsultaCoordenadas
│   ├── Dockerfile
│   ├── nginx.conf
│   └── .env
├── docker-compose.yml
└── README.md
```

---

## Endpoints da API

### Autenticação

| Método | Endpoint     | Descrição          |
|--------|--------------|--------------------|
| POST   | /api/login   | Retorna JWT token  |

**Body:**
```json
{ "email": "clover@aliare.co", "senha": "clover123" }
```

### Consultas de Clima *(requer Bearer token)*

| Método | Endpoint                            | Descrição                          |
|--------|-------------------------------------|------------------------------------|
| POST   | /api/consulta-clima/cidade          | Consulta temperatura por cidade    |
| POST   | /api/consulta-clima/latlong         | Consulta temperatura por lat/long  |
| GET    | /api/consulta-clima/consultaclima   | Histórico dos últimos 30 dias      |

### Utilitários

| Método | Endpoint  | Descrição                        |
|--------|-----------|----------------------------------|
| GET    | /health   | Health check do sistema          |
| GET    | /swagger  | Documentação interativa da API   |

---

## Arquitetura Docker

```
Browser
  │
  ▼
[Frontend - Nginx :80]
  │  serve arquivos estáticos Vue
  │  proxy /api/* → backend:8080
  │
  ▼
[Backend - .NET 8 :8080]
  │  JWT auth
  │  OpenWeatherMap API
  │  EF Core migrations automáticas
  │
  ▼
[Database - PostgreSQL :5432]
  │  volume persistente postgres_data
```

O Nginx faz proxy transparente das chamadas `/api` do browser para o container do backend, sem expor o backend diretamente ao exterior (exceto a porta 8080 para acesso ao Swagger).
