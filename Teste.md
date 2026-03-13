# O desafio
Aplicação full-stack utilizando .NET 8 (C#) no backend 

Consumindo uma API REST e persistindo dados em banco relacional.

A aplicação deve permitir que o usuário consulte e registre informações de clima de diferentes localidades, com visualização de histórico.
- Você pode usar a API do *[OpenWeatherMaps](https://openweathermap.org)* para buscar dados de temperatura;


## Requisitos
- Registrar temperatura por cidade

  - Deve existir um endpoint que receba o nome da cidade.
  - A aplicação deve consultar um provedor de clima (ou simulado/fake provider), persistir o resultado no banco de dados e retornar a temperatura atual.

- Registrar temperatura por coordenadas

  - Deve existir um endpoint que receba a latitude e longitude.
  - A aplicação deve consultar o provedor de clima, persistir o resultado no banco de dados e retornar a temperatura atual.

- Consultar histórico de temperaturas

  - Deve existir um endpoint que receba o nome da cidade ou as coordenadas (lat/long).
  - O sistema deve retornar o histórico de temperaturas registradas para a localidade nos últimos 30 dias, ordenadas do mais recente para o mais antigo.

- Interface Web

  - A aplicação deve possuir um frontend em Vue 3 + TypeScript que permita:
    - Informar o nome da cidade para registrar a leitura de temperatura.
    - Consultar e visualizar o histórico de temperaturas em lista e em gráfico.

## Requisitos não funcionais

- A aplicação deve ser desenvolvida em .NET 8 (C#) no backend e Vue 3 + TypeScript no frontend.


- O sistema deve expor um health check em /health.
- O código deve conter testes automatizados (unitários e pelo menos um de integração).
- A solução deve ser conteinerizada com Docker, com docker-compose.yml para orquestrar API, banco e frontend.
- O repositório deve conter instruções claras no README.md para execução da aplicação.

- Será considerado ponto extra:

 - Feature flag para troca de provedor de clima.



