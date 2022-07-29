

namespace Application.Common.Interfaces
{
    public interface IDapperMonthlyReportHandler
    {
        Task CalculateTransactionPoints(int year, int[] months);
    }
}
