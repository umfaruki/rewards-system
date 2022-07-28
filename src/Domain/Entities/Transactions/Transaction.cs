using Domain.Entities.Customers;

namespace Domain.Entities.Transactions;

public class Transaction
{
    public int Id { get; set; }    
    public DateTime DateCreated { get; set; }
    public decimal MinSpentAmount { get; set; }
    public int MinSpentAmountPoints { get; set; }
    public decimal UpperRangeSpentAmount { get; set; }
    public int UpperRangeSpentPoints { get; set; }
    public decimal SubTotal { get; set; }
    public decimal TotalVAT { get; set; }
    public decimal GrandTotal { get; set; }
    public decimal TaxRate { get; set; }
    public int TotalPoints { get; set; } = 0;
    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; }

    public virtual ICollection<TransactionItem> TransactionItems { get; set; }
}
