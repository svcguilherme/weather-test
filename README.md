# Weather App — .NET 10 + Docker

Aplicação com **API de temperatura** em ASP.NET Core e **frontend em Blazor Server**, containerizada com Docker.

---

## Estrutura do Projeto

```
docker_net/
├── docker-compose.yml
├── WeatherApi/                          ← ASP.NET Core Web API (.NET 10)
│   ├── Controllers/
│   │   ├── WeatherController.cs         ← GET /api/weather?city={cidade}
│   │   └── HealthController.cs          ← GET /api/health
│   ├── Models/WeatherModels.cs
│   ├── Services/WeatherService.cs       ← Consome Open-Meteo (sem chave)
│   ├── Dockerfile
│   └── .dockerignore
└── WeatherFrontend/                     ← Blazor Web App (.NET 10)
    ├── Components/Pages/Weather.razor   ← Página de busca por cidade
    ├── Dockerfile
    └── .dockerignore
```

---

## Endpoints da API

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/weather?city={cidade}` | Retorna temperatura atual da cidade |
| GET | `/api/health` | Verifica se a API está saudável |

### Exemplo de resposta — `/api/weather?city=São Paulo`

```json
{
  "city": "São Paulo",
  "country": "Brazil",
  "latitude": -23.5475,
  "longitude": -46.6361,
  "temperatureCelsius": 23.8,
  "temperatureFahrenheit": 74.8,
  "lastUpdated": "2026-05-13T17:00:00"
}
```

### Exemplo de resposta — `/api/health`

```json
{
  "status": "healthy",
  "timestamp": "2026-05-13T17:00:00Z",
  "version": "1.0.0"
}
```

---

## Rodando Localmente com Docker

### Pré-requisitos

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado e em execução

### 1. Clonar / entrar na pasta do projeto

```bash
cd docker_net
```

### 2. Subir os containers

```bash
docker-compose up --build -d
```

### 3. Acessar

| Serviço | URL |
|---------|-----|
| Frontend Blazor | http://localhost:5051/weather |
| API — weather | http://localhost:5050/api/weather?city=London |
| API — health | http://localhost:5050/api/health |

### 4. Parar os containers

```bash
docker-compose down
```

---

## Publicando no Docker Hub

### 1. Rebuildar as imagens

```bash
docker-compose build --no-cache
```

### 2. Login no Docker Hub

```bash
docker login
```

### 3. Taguear as imagens

```bash
docker tag docker_net-weather-api:latest SEU_USER/weather-api:latest
docker tag docker_net-weather-frontend:latest SEU_USER/weather-frontend:latest
```

### 4. Fazer o push

```bash
docker push SEU_USER/weather-api:latest
docker push SEU_USER/weather-frontend:latest
```

> Imagens publicadas em:
> - https://hub.docker.com/r/guilhermesantannait/weather-api
> - https://hub.docker.com/r/guilhermesantannait/weather-frontend

---

## Rodando em uma VPS

### 1. Instalar Docker (Ubuntu 24.04)

```bash
apt update && apt upgrade -y
curl -fsSL https://get.docker.com | sh
apt install docker-compose-plugin -y
```

### 2. Criar o docker-compose.yml na VPS

```bash
cat > docker-compose.yml << 'EOF'
services:
  weather-api:
    image: guilhermesantannait/weather-api:latest
    ports:
      - "5050:5050"
    restart: unless-stopped
    networks:
      - weather-net

  weather-frontend:
    image: guilhermesantannait/weather-frontend:latest
    ports:
      - "5051:5051"
    environment:
      - WeatherApi__BaseUrl=http://weather-api:5050
    depends_on:
      - weather-api
    restart: unless-stopped
    networks:
      - weather-net

networks:
  weather-net:
    driver: bridge
EOF
```

### 3. Subir

```bash
docker compose up -d
```

Acesse por `http://IP_DA_VPS:5051/weather`.

---

## VPS Recomendadas

| VPS | Plano mínimo | Preço/mês | Destaque |
|-----|-------------|-----------|----------|
| **Hetzner Cloud** | CX22 — 2 vCPU / 4GB | ~€4,50 | Melhor custo-benefício |
| **Vultr** | 1 vCPU / 1GB | US$6 | Data center em São Paulo |
| **DigitalOcean** | 1 vCPU / 1GB | US$6 | Interface simples |
| **Contabo** | 4 vCPU / 4GB | ~€5 | Mais recursos pelo preço |

---

## Tecnologias

- [.NET 10](https://dotnet.microsoft.com/)
- [ASP.NET Core Web API](https://learn.microsoft.com/aspnet/core/web-api/)
- [Blazor Web App](https://learn.microsoft.com/aspnet/core/blazor/) (Interactive Server)
- [Open-Meteo API](https://open-meteo.com/) — dados de temperatura gratuitos, sem chave
- [Docker](https://www.docker.com/) + [Docker Compose](https://docs.docker.com/compose/)
