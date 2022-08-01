# Rewards System


## Problem Statement

A retailer offers a rewards program to its customers, awarding points based on each recorded purchase as follows:

For every dollar spent over $50 on the transaction, the customer receives one point.

In addition, for every dollar spent over $100, the customer receives another point.

Ex: for a $120 purchase, the customer receives

`(120 - 50) x 1 + (120 - 100) x 1 = 90 points`


Given a record of every transaction during a three-month period, calculate the reward points earned for each customer per month and total. 




## Introduction



I based my solution on `Clean Architecture`. For more information please visit https://github.com/jasontaylordev/CleanArchitecture

I am using a code-first approach with Asp.Net Core 6.0 with EntityFramework core and Dapper, using EntityFramework migrations to create a database.

- Using Dapper to calculate points with update queries (can be achieved using EntityFramework)
- Postgres as a database
- pgAdmin (No need to install, it will run with docker and accessed from the browser)
- Docker
- Polly
- HealthCheck: https://localhost:8005/health

## How to Run:

_**Docker**_

We are using Docker, please make sure docker is installed on your system.
If using windows then open the command prompt and navigate to the solution folder.

Run the following commands:

```
1. Build: docker-compose -f docker-compose.yml -f docker-compose.override.yml build
2. Run:   docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
```

>To run the API please navigate to http://localhost:8002/swagger 



For database administration we are using pgAdmin you access it directly by navigating to the link below

>pdAdmin: http://localhost:5000
with `username: libra_dn@hotmail.com` and `password: admin1234`


 _**Without Docker**_

If you want to run it without docker, `PostgreSQL` is required, 
Just update the connection string in RewardsApi/appsettings.js and run the API.




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
## Application Structure:
There is only one controller in system TransactionController with the method below
```
        [HttpGet]
        public async Task<ActionResult<List<MonthlyPointsReportDto>>> MonthlyPointsReports([FromQuery] MonthlyPointsReportQuery query)
        {
            return await Mediator.Send(query);
        }
```
When it is called it sends a call to `MonthlyPointsReportQueryHandler` via Mediator. Call received by Handle method in Handler.
The below code is responsible for generating reward points and returning them.
```
 public async Task<List<MonthlyPointsReportDto>> Handle(MonthlyPointsReportQuery request, CancellationToken cancellationToken)
    {
        await _dapperTransactionReportHandler.CalculateTransactionPoints(request.Year, request.Months);
        var transactionData = await _dapperTransactionReportHandler.GetTransactionData(request.Year, request.Months);
        var monthlyPointsReports = transactionData.GenerateReport(request);
        return monthlyPointsReports;
    }
```
From the above code ` await _dapperTransactionReportHandler.GetTransactionData(request.Year, request.Months);` send a call to Dapper handler to calculate points
>Below is the code where we are calculating the points:
![Points Calculation](https://i.postimg.cc/Bnt0d4kx/points-calculation.png)

After calculating the points system send the call to `transactionData.GenerateReport(request)` to generate a report which we are going to return to the API request.
We are using an Extention method to generate response `MonthlyPointsExtentions`

```
public static List<MonthlyPointsReportDto> GenerateReport(this List<RawTransactionsData> transactionData, MonthlyPointsReportQuery request)
    {
        var monthlyPointsReports = new List<MonthlyPointsReportDto>();

        var uniqueCustomers = transactionData.Select(x => new { x.CustomerId, x.Firstname, x.Surname, x.CustomerNumber }).Distinct().ToList();

        uniqueCustomers.ForEach(customer =>
        {
            var customerReport = new MonthlyPointsReportDto();
            customerReport.CustomerId = customer.CustomerId;
            customerReport.FirstName = customer.Firstname;
            customerReport.Surname = customer.Surname;
            customerReport.CustomerNumber = customer.CustomerNumber;

            request.Months.ToList().ForEach(month =>
            {
                var customerMonthlyData = transactionData.FirstOrDefault(x => x.Year == request.Year && x.Month == month && x.CustomerId == customer.CustomerId);
                if (customerMonthlyData != null)
                {
                    customerReport.MonthlyPoints.Add(new MonthlyPointsReportVm
                    {
                        Month = month,
                        MonthAndYear = $"{month}/{request.Year.ToString()}",
                        Year = request.Year,
                        MonthlyTotal = customerMonthlyData.TotalPoints,
                    });
                }
            });
            customerReport.TotalPoints = customerReport.MonthlyPoints.Sum(x => x.MonthlyTotal);
            monthlyPointsReports.Add(customerReport);
        });
        return monthlyPointsReports;
    }
```
The idea is to **save point calculation information with each transaction**, so then, sometimes if the vendor changes the criteria (e.g. 2 points for more than 100), then old transactions remain intact.

`In real world scenario, at the time of saving transaction, we should calculate the points and save them to the database. For the assignment, I am calculating it when I call API endpoint.`


**Seeding Data** 
>I am seeding data from Mock JSON files, I am using 3 JSON files and my code logic randomly assigns transaction items to each transaction. If you need to test anything there are two ways either update the database directly and call the API end-point or update the JSON data.
JSON files are created by the online tool [Mockaroo](https://www.mockaroo.com/)
 /src/Rewards.Api/DataSeed
 
![json-files.png](https://i.postimg.cc/5y2g24k8/json-files.png)

I am reading these from one generic method
ApplicationDbContextInitializer.cs
```
    var customerList = MockDataHelper.GetMockData<Customer>();
    var transactionsList = MockDataHelper.GetMockData<Transaction>();
    var transactionsItemsList = MockDataHelper.GetMockData<TransactionItem>();
```
![mockhelper.png](https://i.postimg.cc/VshnCWJx/mockhelper.png)

Below is the complete logic to seed data.
For each customer, the system loops on transactions and assigns 10 transactions per customer
The system randomly (1 to 10) assigns products to each transaction and calculates the price according to the assigned product quantity.
![seed-data.png](https://i.postimg.cc/cHr7p9Kc/seed-data.png)

## Dockerization

Created a docker file in the RewardsApi folder

![ApiDocker](https://i.postimg.cc/mD75M8MY/docker-api.png)

Added files `docker-compose.yml` and `docker-compose.override.yml`

![files](https://i.postimg.cc/QM2zHRBN/docker-files.png)

>These contain configuration for running PostgreSQL and pgAdmin configuring ports setting up credentials

_docker-compose.yml_

![Docker](https://i.postimg.cc/K8swyKxM/docker.png)

_docker-compose.override.yml_
![Override](https://i.postimg.cc/gjdBK8qf/docker-override.png)

## Request:

The following request will bring the data for the year 2022 and months 5 and 6

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




