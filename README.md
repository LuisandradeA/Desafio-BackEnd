# Desafio backend.
Seja muito bem-vindo ao desafio backend, obrigado pelo interesse em fazer parte do nosso time e ajudar a melhorar a vida de milhares de pessoas.

# Visão Geral
Esta aplicação foi desenvolvida utilizando .NET 8 e segue princípios modernos de arquitetura, visando escalabilidade, manutenibilidade e facilidade de integração com outros sistemas.
##Organização do Projeto
O projeto está estruturado em múltiplos módulos, cada um com responsabilidade bem definida:
- API: Responsável por expor endpoints RESTful para consumo externo.
- Domínio: Contém as entidades de negócio, regras e validações.
- Infraestrutura: Implementa o acesso a dados, integrações externas e configurações de serviços.
- Consumers: Serviços que consomem mensagens de filas (RabbitMQ).
- Repositórios: Abstração e implementação do acesso a dados (relacional e NoSQL).
## Banco de Dados Relacional (PostgreSQL)
A aplicação utiliza PostgreSQL como banco de dados relacional principal. O acesso é realizado via Entity Framework Core, utilizando o padrão Code First. As entidades do domínio são mapeadas para tabelas, e as migrações são gerenciadas pelo EF Core, facilitando a evolução do schema.
## Entity Framework Core (Code First)
O EF Core é utilizado para mapear as classes do domínio para o banco de dados. As migrações são criadas e aplicadas via comandos do EF Core, garantindo que o banco esteja sempre sincronizado com o modelo da aplicação.
## Mensageria com RabbitMQ
A comunicação assíncrona entre serviços é realizada via RabbitMQ. Os consumers escutam filas específicas e processam eventos recebidos, promovendo desacoplamento e escalabilidade.
## Validações com FluentValidation
O FluentValidation é utilizado para validar objetos de entrada (DTOs, comandos, etc.) de forma centralizada e desacoplada das entidades. As regras de validação são implementadas em classes específicas, facilitando manutenção e testes.
## Padrões e Boas Práticas
- Injeção de Dependência: Utilizada em toda a aplicação para promover baixo acoplamento.
- Separação de Responsabilidades: Cada camada/módulo tem uma responsabilidade clara.
- Configuração via appsettings: Strings de conexão, configurações de RabbitMQ e outros parâmetros são definidos via arquivos de configuração.
## Como Executar
1.	Pré-requisitos
- Certifique-se de ter o Docker instalado em sua máquina.
- Tenha o .NET 8 SDK instalado para executar os projetos localmente.
2.	Subindo os recursos necessários
- No diretório raiz do projeto, execute o comando abaixo para subir os containers do PostgreSQL, RabbitMQ e demais dependências:
docker-compose up --build -d
3.	Executando o Consumer
- Execute o consumer com o comando:
 cd ./DeliveryApp.CreatedMotorcycleEventConsumer
- Execute o consumer com o comando:
dotnet run
4.	Executando a API
- Você pode executar a API normalmente pelo Visual Studio 2022 (F5) ou via terminal:
 cd ./DevliveryApp
 dotnet run
6.	Migrações do Banco de Dados
- As migrações serão aplicadas ao rodar a api. Caso necessário, execute as migrações do Entity Framework Core para garantir que o banco esteja atualizado:
dotnet ef database update


  

