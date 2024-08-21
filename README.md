# MyRentMotorService - Motorcycle Rental System

## Overview

MyRentMotorService is a solution composed of three main microservices, each responsible for a specific aspect of the motorcycle rental system. These microservices are integrated via RabbitMQ and have their own separate databases. The system's architecture is designed to offer a modular, scalable, and independent service, where each microservice plays a crucial role in the rental process.

### Microservices

1. **MotorcycleService**: Responsible for registering and managing motorcycles. This service handles operations such as adding, updating, retrieving, and removing motorcycles.

2. **CustomerService**: Responsible for registering and managing customers. This service includes operations for adding, updating, retrieving, and removing customers, as well as managing driver's license (CNH) images using cloud storage services.

3. **RentMotorService**: Responsible for creating and managing motorcycle rentals. This service connects motorcycles and customers, calculates costs, provides cost previews, and finalizes rentals. It also sends signals to the customer and motorcycle microservices to activate records when a rental is created.

### Microservices Communication

The communication between microservices is done through RabbitMQ. When a customer or motorcycle is registered, the respective events are published, and the other services consume these events to update their data and process the necessary information.

- **Customer Registration**: When a new customer is registered, the rental microservice receives an event indicating the new registration.
- **Motorcycle Registration**: When a new motorcycle is registered, the rental microservice is notified to include the motorcycle as available for rental.

### Rental Plans

The following plans can be added at the time of rental:

- **SevenDaysPlan**
- **FifteenDaysPlan**
- **ThirtyDaysPlan**
- **FortyFiveDaysPlan**
- **FiftyDaysPlan**

### Driver's License Types

There are three types of driver's licenses: A, B, and AB. However, only holders of types **A** and **AB** are eligible to rent motorcycles.

### Microservices Structure

#### MotorcycleService

- **Endpoints**:
  - `GET /api/motorcycles/{licensePlate}`: Retrieves motorcycle information based on the license plate.
  - `GET /api/motorcycles`: Retrieves a list of all motorcycles.
  - `POST /api/motorcycles`: Adds a new motorcycle to the system.
  - `PUT /api/motorcycles/{licensePlate}`: Updates the information of an existing motorcycle.
  - `DELETE /api/motorcycles/{licensePlate}`: Removes a motorcycle from the system.
  - `PATCH /api/motorcycles/motorcycle/{licensePlate}`: Partially updates the license plate of a motorcycle.

- **Swagger**: The Swagger UI for the MotorcycleService is available at [https://localhost:5003/swagger/index.html](https://localhost:5003/swagger/index.html).

#### CustomerService

The **CustomerService** also integrates cloud storage services using **AWS S3** to manage driver's license (CNH) images.

- **Endpoints**:
  - `GET /api/customers/{driverLicense}`: Retrieves customer information based on the driver's license (CNH).
  - `GET /api/customers/cnpj/{cnpj}`: Retrieves customer information based on the CNPJ.
  - `GET /api/customers`: Retrieves a list of all customers.
  - `POST /api/customers`: Adds a new customer to the system.
  - `PUT /api/customers/{driverLicense}`: Updates the information of an existing customer by driver's license (CNH).
  - `PUT /api/customers/cnpj/{cnpj}`: Updates the information of an existing customer by CNPJ.
  - `DELETE /api/customers/{driverLicense}`: Removes a customer from the system by driver's license (CNH).
  - `DELETE /api/customers/cnpj/{cnpj}`: Removes a customer from the system by CNPJ.
  - `POST /api/customers/{driverLicense}/driverlicenseimage`: Uploads the customer's driver's license image to AWS S3.
  - `GET /api/customers/{driverLicense}/driverlicenseimage`: Downloads the customer's driver's license image from AWS S3.
  - `DELETE /api/customers/{driverLicense}/driverlicenseimage`: Removes the customer's driver's license image from AWS S3.

- **Swagger**: The Swagger UI for the CustomerService is available at [https://localhost:5005/swagger/index.html](https://localhost:5005/swagger/index.html).

#### RentMotorService

- **Endpoints**:
  - `POST /api/rentals`: Creates a new motorcycle rental.
  - `POST /api/rentals/{id}/complete`: Completes an existing rental.
  - `GET /api/rentals/{id}`: Retrieves the details of a specific rental.
  - `GET /api/rentals`: Retrieves all existing rentals.
  - `POST /api/rentals/driverlicense/{driverLicense}/preview`: Provides a cost preview for a rental based on the customer's driver's license (CNH).

- **Swagger**: The Swagger UI for the RentMotorService is available at [https://localhost:5001/swagger/index.html](https://localhost:5001/swagger/index.html).

### Environment Configuration

For the driver's license service to function correctly, it is necessary to configure the AWS S3 bucket access credentials and bucket name.

1. **Edit the `appsettings.Development.json`**: Add the following entry with your AWS S3 bucket name:

   ```json
   "AWS": {
     "BucketName": "your-bucket-name"
   }
   ```
2. **Replace `"your-bucket-name"`** with the actual name of your AWS S3 bucket.

### Update the `.env` File
Include the following environment variables in your `.env` file or create it, which should be located in `[RootFolder]\MyCustomerService.API\MyCustomerService.API`:

```bash
AWS_ACCESS_KEY_ID=your-access-key-id
AWS_SECRET_ACCESS_KEY=your-secret-access-key
AWS_REGION=your-region
```

3. **Replace `your-access-key-id`, `your-secret-access-key`, and `your-region` with your actual AWS credentials.**

**Important**: For security reasons, the AWS S3 access credentials have been removed from this repository. To run the CNH images repository, you need to edit the `.env` file with your own AWS credentials, such as access keys and bucket name, **but the application can be executed without this feature.**

### How to Run the Application

To run the application, follow these steps:

1. **Clone the Repository**: Clone this repository to your local environment.

2. **Navigate to the ROOT Docker Compose Directory**: Navigate to the **root** directory where the main `docker-compose.yml` file is located. The same directory as MyCustomerService.API, MyMotorcycleService, MyRentMotorService and Shared folders are located.

3. **Build and Start the Containers**: Run the following command to build the images and start the containers without using the previous cache:

```bash
docker-compose build --no-cache
```
4. After all containers are built and started, the services will be available on the configured ports. You can use tools like Postman, cURL, or directly access the Swagger UIs to interact with the services:

- **CustomerService Swagger**: [https://localhost:5005/swagger/index.html](https://localhost:5005/swagger/index.html)
- **MotorcycleService Swagger**: [https://localhost:5003/swagger/index.html](https://localhost:5003/swagger/index.html)
- **RentMotorService Swagger**: [https://localhost:5001/swagger/index.html](https://localhost:5001/swagger/index.html)

The integration with AWS S3 for managing driver's license images demonstrates the ability to use cloud computing solutions, ensuring that the system is scalable and secure. The modular structure allows for easy maintenance and the possibility of expanding the services as needed without significantly impacting the overall system.



# Desafio backend Mottu.
Seja muito bem-vindo ao desafio backend da Mottu, obrigado pelo interesse em fazer parte do nosso time e ajudar a melhorar a vida de milhares de pessoas.

## Instruções
- O desafio é válido para diversos níveis, portanto não se preocupe se não conseguir resolver por completo.
- A aplicação só será avaliada se estiver rodando, se necessário crie um passo a passo para isso.
- Faça um clone do repositório em seu git pessoal para iniciar o desenvolvimento e não cite nada relacionado a Mottu.
- Após finalização envie um e-mail para o recrutador informando o repositório para análise.
  
## Requisitos não funcionais 
- A aplicação deverá ser construida com .Net utilizando C#.
- Utilizar apenas os seguintes bancos de dados (Postgress, MongoDB)
    - Não utilizar PL/pgSQL
- Escolha o sistema de mensageria de sua preferencia( RabbitMq, Sqs/Sns , Kafka, Gooogle Pub/Sub ou qualquer outro)

## Aplicação a ser desenvolvida
Seu objetivo é criar uma aplicação para gerenciar aluguel de motos e entregadores. Quando um entregador estiver registrado e com uma locação ativa poderá também efetuar entregas de pedidos disponíveis na plataforma.
### Casos de uso
- Eu como usuário admin quero cadastrar uma nova moto.
  - Os dados obrigatórios da moto são Identificador, Ano, Modelo e Placa
  - A placa é um dado único e não pode se repetir.
  - Quando a moto for cadastrada a aplicação deverá gerar um evento de moto cadastrada
    - A notificação deverá ser publicada por mensageria.
    - Criar um consumidor para notificar quando o ano da moto for "2024"
    - Assim que a mensagem for recebida, deverá ser armazenada no banco de dados para consulta futura.
- Eu como usuário admin quero consultar as motos existentes na plataforma e conseguir filtrar pela placa.
- Eu como usuário admin quero modificar uma moto alterando apenas sua placa que foi cadastrado indevidamente
- Eu como usuário admin quero remover uma moto que foi cadastrado incorretamente, desde que não tenha registro de locações.
- Eu como usuário entregador quero me cadastrar na plataforma para alugar motos.
    - Os dados do entregador são( identificador, nome, cnpj, data de nascimento, número da CNHh, tipo da CNH, imagemCNH)
    - Os tipos de cnh válidos são A, B ou ambas A+B.
    - O cnpj é único e não pode se repetir.
    - O número da CNH é único e não pode se repetir.
- Eu como entregador quero enviar a foto de minha cnh para atualizar meu cadastro.
    - O formato do arquivo deve ser png ou bmp.
    - A foto não poderá ser armazenada no banco de dados, você pode utilizar um serviço de storage( disco local, amazon s3, minIO ou outros).
- Eu como entregador quero alugar uma moto por um período.
    - Os planos disponíveis para locação são:
        - 7 dias com um custo de R$30,00 por dia
        - 15 dias com um custo de R$28,00 por dia
        - 30 dias com um custo de R$22,00 por dia
        - 45 dias com um custo de R$20,00 por dia
        - 50 dias com um custo de R$18,00 por dia
    - A locação obrigatóriamente tem que ter uma data de inicio e uma data de término e outra data de previsão de término.
    - O inicio da locação obrigatóriamente é o primeiro dia após a data de criação.
    - Somente entregadores habilitados na categoria A podem efetuar uma locação
- Eu como entregador quero informar a data que irei devolver a moto e consultar o valor total da locação.
    - Quando a data informada for inferior a data prevista do término, será cobrado o valor das diárias e uma multa adicional
        - Para plano de 7 dias o valor da multa é de 20% sobre o valor das diárias não efetivadas.
        - Para plano de 15 dias o valor da multa é de 40% sobre o valor das diárias não efetivadas.
    - Quando a data informada for superior a data prevista do término, será cobrado um valor adicional de R$50,00 por diária adicional.
    

## Diferenciais 🚀
- Testes unitários
- Testes de integração
- EntityFramework e/ou Dapper
- Docker e Docker Compose
- Design Patterns
- Documentação
- Tratamento de erros
- Arquitetura e modelagem de dados
- Código escrito em língua inglesa
- Código limpo e organizado
- Logs bem estruturados
- Seguir convenções utilizadas pela comunidade
  

