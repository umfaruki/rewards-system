

using Application.Common.Model;

namespace Application.Common.Interfaces
{
    public interface IDapperMonthlyReportHandler
    {
        Task CalculateTransactionPoints(int year, int[] months);
        Task<List<RawTransactionsData>> GetTransactionData(int year, int[] months);
    }
}
