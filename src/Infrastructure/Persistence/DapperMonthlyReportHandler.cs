﻿using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Dapper;
using Npgsql;


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
                ("UPDATE public.\"Transactions\" Set TotalPoints = 0 Where SubTotal < MinSpentAmount AND Year(DateCreated)=@Year and Month(DateCreated) In (@Months)",
                    new { Year = year, Months = string.Join(",", months) });



        }



        private async Task CalculatePointsGreaterThanUpperSpent(int year, int[] months)
        {
            using var connection = new NpgsqlConnection
                (_configuration.GetConnectionString("DefaultConnection"));

            var affected =
                await connection.ExecuteAsync
                ("UPDATE public.\"Transactions\" Set TotalPoints = ((SubTotal - MinSpentAmount) * MinSpentAmountPoints) + ((SubTotal - UpperRangeSpentAmount) * UpperRangeSpentPoints) Where SubTotal > UpperRangeSpentAmount AND Year(DateCreated)=@Year and Month(DateCreated) In (@Months)",
                    new { Year = year, Months = string.Join(",", months) });



        }


        private async Task CalculatePointsGreaterThanLowerSpent(int year, int[] months)
        {
            using var connection = new NpgsqlConnection
                (_configuration.GetConnectionString("DefaultConnection"));

            var affected =
                await connection.ExecuteAsync
                ("UPDATE public.\"Transactions\" Set TotalPoints = ((SubTotal - MinSpentAmount) * MinSpentAmountPoints) + ((SubTotal - UpperRangeSpentAmount) * UpperRangeSpentPoints) Where SubTotal > UpperRangeSpentAmount AND Year(DateCreated)=@Year and Month(DateCreated) In (@Months)",
                    new { Year = year, Months = string.Join(",", months) });

          

        }


        public async Task CalculateTransactionPoints(int year, int[] months)
        {
            await SetUnQualifiedTransactions(year, months);
            await CalculatePointsGreaterThanLowerSpent(year, months);
            await CalculatePointsGreaterThanUpperSpent(year, months);
        }
    }
}
