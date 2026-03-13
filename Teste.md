# O desafio
Aplicação full-stack utilizando .NET 8 (C#) no backend 


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



