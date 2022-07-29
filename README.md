# Rewards System





## Problem Statement



A retailer offers a rewards program to its customers, awarding points based on each recorded purchase as follows:

 

For every dollar spent over $50 on the transaction, the customer receives one point.

In addition, for every dollar spent over $100, the customer receives another point.

Ex: for a $120 purchase, the customer receives

`(120 - 50) x 1 + (120 - 100) x 1 = 90 points`



Given a record of every transaction during a three-month period, calculate the reward points earned for each customer per month and total. 





## Introduction



I based my solution on Clean Architecture. For more information please visit https://github.com/jasontaylordev/CleanArchitecture

I am using a code first approach with Asp.Net Core 6.0 with EntityFramework core and Dapper, using EntityFramework migrations to create a database.

- Using Dapper to calculate points with update queries (can be achieved using EntityFramework)
- Postgres as database
- pgAdmin (No need to install, it will run with docker and accessed from browser)
- Docker
- Polly
- HealthCheck: https://localhost:8005/health

## How to Run:



We are using Docker, please make sure docker is installed on your system.

If using windows then open command prompt and navigate to solution folder


Run the following commands

```
1. Build: docker-compose -f docker-compose.yml -f docker-compose.override.yml build

2. Run:      docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d

```

>To run the Api please navigate to http://localhost:8002/swagger 



For database administration we are using pgAdmin you access it directly by navigating to link below

>pdAdmin: http://localhost:5000
with username: libra_dn@hotmail.com and password: admin1234







## Database Structure:



By understanding the problem statement, we have created 3 tables

>![Tables](https://i.postimg.cc/3rCYGL2D/tables.png)

**Customers** - stores the customer information

Columns:

```
    Id
    CustomerNumber
    DateJoined
    Firstname
    Surname
    Email
    Telephone
```
>![Customers](https://i.postimg.cc/T1GxSmvw/customers.png)


**Transactions** - stores the customer transactions against customers, customerId is used as foregionKey from customers customer's table

Columns:
``` 
    Id    
    DateCreated
    MinSpentAmount
    MinSpentAmountPoints
    UpperRangeSpentAmount
    UpperRangeSpentPoints
    SubTotal
    TotalVAT
    GrandTotal
    TaxRate
    TotalPoints
    CustomerId
```
>![Transactions](https://i.postimg.cc/cJnNLTsC/transactions.png)

**TransactionItems** - stores the number of transaction-items per transaction against each transaction. TransactionId is used as a foreignKey here 

Columns: 
```
     Id
     ItemDescription
     Price  
     Quantity 
     TransactionId (as foreign key from transactions table)
```
>![TransactionItems](https://i.postimg.cc/7YK4sfsH/transactionitems.png)

**RewardSettings** - Just includes a single record with the reward settings (e.g. Reward point calculations limits from problem statement)

Columns:
```
    MinSpentAmount          = 50
    MinSpentAmountPoints    = 1
    UpperRangeSpentAmount   = 100
    UpperRangeSpentPoints   = 1
    TaxRate                 = 20
```




## Business Logic:


The idea is to save point calculation information with each transaction, so then, sometimes if the vendor changes the criteria (e.g. 2 points for more than 100), then old transactions remain intact.

a realIn real world scenario, at the time of saving transaction, we should calculate the points and save to the database. For assignment, I am calculating it when I call api-endpoint.

>Below is the code where we are calculating the points:
![Points Calculation](https://i.postimg.cc/Bnt0d4kx/points-calculation.png)

**Seeding Data** 
>I am seeding data from Mock Json files, I am using 3 json files and my code logic randomly assigning transaction items to each transaction. If you need to test anything there are two ways either update the database directly and call api end-point or update the json data.


## Request:

Following request will bring the data for year 2022 and for months 5 and 6



```
https://localhost:8005/api/Transaction?Year=2022&Months=5&Months=6'

```

## Response:

something like below

> 

```

[   
    {
        "customerId": 5,
        "customerNumber": "50-4606430",
        "firstName": "Ingrim",
        "surname": "Erasmus",
        "monthlyPoints": [
            {
                "month": 5,
                "year": 2022,
                "monthAndYear": "5/2022",
                "monthlyTotal": 90
            }
        ],
        "totalPoints": 90
    },

]

```


Request/Response in Postman

![Postman Call](https://i.postimg.cc/fTzZfxvq/postman.png)




