using Domain.Entities.Customers;

namespace Domain.Entities.Transactions;

public class Transaction
{
    public int Id { get; set; }    
    public int DateCreated { get; set; }
    public int MinSpentAmount { get; set; }
    public int MinSpentAmountPoints { get; set; }
    public int UpperRangeSpentAmount { get; set; }
    public int UpperRangeSpentPoints { get; set; }
    public int SubTotal { get; set; }
    public int TotalVAT { get; set; }
    public int GrandTotal { get; set; }
    public int TaxRate { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
}
