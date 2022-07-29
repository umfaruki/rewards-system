using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Dapper;
using Npgsql;
using Application.Common.Model;

namespace Infrastructure.Persistence
{
    public class DapperMonthlyReportHandler : IDapperMonthlyReportHandler
    {
        private readonly IConfiguration _configuration;
        public DapperMonthlyReportHandler(IConfiguration configuration)
        {
            _configuration = configuration;


        }


        private async Task SetUnQualifiedTransactions(int year, int[] months)
        {
            using var connection = new NpgsqlConnection
                (_configuration.GetConnectionString("DefaultConnection"));

            var affected =
                await connection.ExecuteAsync
                ("UPDATE Transactions Set TotalPoints = 0 Where SubTotal < MinSpentAmount AND date_part('year', DateCreated)=@Year and date_part('month', DateCreated) = Any(@Months)",
                    new { Year = year, Months = months });
        }



        private async Task CalculatePointsGreaterThanUpperSpent(int year, int[] months)
        {
            using var connection = new NpgsqlConnection
                (_configuration.GetConnectionString("DefaultConnection"));

            var affected =
                await connection.ExecuteAsync
                (@"UPDATE Transactions Set TotalPoints = ((SubTotal - MinSpentAmount) * MinSpentAmountPoints) 
                        Where SubTotal > MinSpentAmount And SubTotal < UpperRangeSpentAmount AND date_part('year', DateCreated)=@Year and date_part('month', DateCreated) = Any(@Months)",
                    new { Year = year, Months = months });
        }


        private async Task CalculatePointsGreaterThanLowerSpent(int year, int[] months)
        {
            using var connection = new NpgsqlConnection
                (_configuration.GetConnectionString("DefaultConnection"));

            var affected =
                await connection.ExecuteAsync
                (@"UPDATE Transactions Set TotalPoints = ((SubTotal - MinSpentAmount) * MinSpentAmountPoints) + ((SubTotal - UpperRangeSpentAmount) * UpperRangeSpentPoints) 
                    Where SubTotal > UpperRangeSpentAmount AND date_part('year', DateCreated)=@Year and date_part('month', DateCreated) = Any(@Months)",
                    new { Year = year, Months = months }); 
        }

        public async Task<List<RawTransactionsData>> GetTransactionData(int year, int[] months)
        {
            using var connection = new NpgsqlConnection
                (_configuration.GetConnectionString("DefaultConnection"));

            var data =
                await connection.QueryAsync<RawTransactionsData>
                (@"SELECT transactions.customerid, customers.customernumber, customers.firstname, customers.surname, date_part('year', transactions.DateCreated) AS year, date_part('month', transactions.DateCreated) as month, sum(transactions.totalpoints) as totalpoints
                     FROM transactions
	                 INNER JOIN customers ON customers.id = transactions.customerid	 
	                 where date_part('year', transactions.DateCreated)=@Year and date_part('month', transactions.DateCreated) = Any(@Months)	 
	                 GROUP BY year,month, customerid,  customers.firstname, customers.surname, customers.customernumber",
                    new { Year = year, Months = months });
            return data.ToList();    
        }


        public async Task CalculateTransactionPoints(int year, int[] months)
        {
            await SetUnQualifiedTransactions(year, months);
            await CalculatePointsGreaterThanLowerSpent(year, months);
            await CalculatePointsGreaterThanUpperSpent(year, months);
        }
    }
}
