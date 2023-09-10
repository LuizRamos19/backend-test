# Process

Fork the repository into your account. Once your code is ready open a pull-request on this repository and we will review it.

# Test

Develop a mini-application to process multiple sales transactions:

1. Create a REST API that should receive by request all the transactions to be processed:
    1. The API should receive the transaction data and send to a Kafka topic.
2. Create a Worker Service to consume the message produced by the API to the Kafka, process each transaction and convert the transaction amount (TransactionAmount) in American dollar (USD):
    1. Use the API https://v6.exchangerate-api.com/v6/{API_KEY}/latest/USD and the column (TransactionCurrencyCode) from the sample file for the exchange rate conversion. 
	   Redis data should expire according to the value returned from the API in the property time_next_update_utc and calculated according to the current datetime of the process being executed. 
	   Example: If the Worker execute at "15/03/2023 23:00:01 +0000" and the response value of the API was "16/03/2023 00:00:01 +0000", the expiration time in Redis should be 1 hour
    2. The Worker should persist the transaction data in the Oracle database

# Technical Requirements

- Use Swagger for API documentation
- Use CQRS concepts
- Use best practices (https://vaibhavbhapkarblogs.medium.com/c-best-practices-48e984cfdac0)
- Ensure performance for both API and Worker
- .Net Core version 3.1
- ORM: Dapper and/or Entity Framework
- Ensure good cyclomatic complexity in the methods

# Bonus

- Unit tests for the API
- Integration tests for the Worker Service
- Use procedures in Oracle to help with the ingestion process in the Worker
- Use logging conscientiously to track what is happening in the process

# Help

Exchange Rate API
- To generate the API_KEY, signup in https://app.exchangerate-api.com/sign-up

WSL2 + Docker installation
- https://github.com/barrosohub/autoinstaller_wsl2_with_ubuntu_20_04_and_docker
- If you face any issue during the installation, make sure both features below are enabled in "Turn Windows features on or off"
    - Microsoft-Windows-Subsystem-Linux
    - VirtualMachinePlatform

Docker compose for Redis, Oracle and Kafka (`Oracle container take a few minutes to start!`)
- [docker-compose.yml](docker-compose.yml)
- After docker compose execution, the services can be accessed by the following tools:
    - For Oracle connection, you can use [Dbeaver](https://dbeaver.io/download/)
        - Host: localhost
        - Port: 1521
        - Database: XE (SID)
        - Username: system
        - Password: Teste123456
    - For Redis connection, you can use [RedisInsight](https://redis.com/redis-enterprise/redis-insight/#insight-form)
        - Host: 127.0.0.1
        - Port: 6379
    - For Kafka connection, access http://localhost:9021/

Data sample for the API
- [Sales.txt](Sales.txt)

Database
- Use the files inside /scripts folder to create the database structure