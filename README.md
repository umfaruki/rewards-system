# Rewards System


## Problem Statement

A retailer offers a rewards program to its customers awarding points based on each recorded purchase as follows:
 
For every dollar spent over $50 on the transaction, the customer receives one point.
In addition, for every dollar spent over $100, the customer receives another point.
Ex: for a $120 purchase, the customer receives
(120 - 50) x 1 + (120 - 100) x 1 = 90 points

Given a record of every transaction during a three-month period, calculate the reward points earned for each customer per month and total. 


## Introduction

I based my solution on Clean Architecture, for more information please visit https://github.com/jasontaylordev/CleanArchitecture
Asp.Net Core 6.0 with EntityFramework core and Dapper
I am using code first approach and using EntityFramework migrations to create database
- Using Dapper to calculate points with update query (can be achieved using EntityFramework)
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
2. Run:	  docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
```
To run the Api please navigate to http://localhost:8002/swagger 

For database administration we are using pgAdmin you access it directly by navigating to link below
pdAdmin: http://localhost:5000
with username: libra_dn@hotmail.com and password: admin1234



## Database Structure:

By understanding the problem statement we have created 3 tables

Customers - stores the customer information
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
Transactions - stores the customer transactions against customers, customerId is used as foregionKey from customers table
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
TransactionItems - stores the number of transaction-items per transaction against each transaction, transactionId is used as foregionKey here 
Columns:	 
 ```
     Id
     ItemDescription
     Price  
     Quantity 
     TransactionId (as foregion key from transactions table)
```
RewardSettings - Just contains the a single record with the reward settings (e.g. Reward point calculations limits from problem statement)
Columns:
```
  	MinSpentAmount		= 50
	MinSpentAmountPoints	= 1
	UpperRangeSpentAmount	= 100
	UpperRangeSpentPoints	= 1
	TaxRate			= 20
```


## Business Logic:

Idea is to save point calculation information with each transaction so after sometimes if vendor changes the criteria (e.g. 2 points on morethan 100) then old transactions remain intact.
In real world scenario at time of saving transaction we should calculate the points and save into database, be for assignment I am calculating it when we are calling api-endpoint.


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
        "customerNumber": "50-4606430",
        "firstName": "Ingrim",
        "surname": "Erasmus",
        "monthlyPoints": [
            {
                "month": 5,
                "year": 2022,
                "customerId": 5,
                "monthAndYear": "5/2022",
                "monthlyTotal": 90
            }
        ],
        "totalPoints": 90
    },
]
```



