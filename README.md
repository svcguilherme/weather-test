# weather-test

## Docker Hub publish commands

After building the local images (`weather-api:latest` and `weather-frontend:latest`), tag and push them to Docker Hub:

```bash
docker tag weather-api:latest SEU_USER/weather-api:latest
docker tag weather-frontend:latest SEU_USER/weather-frontend:latest

docker push SEU_USER/weather-api:latest
docker push SEU_USER/weather-frontend:latest
```
