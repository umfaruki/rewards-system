namespace Domain.Entities.Transactions;

public class TransactionItem
{
    public int Id { get; set; }
    public string ItemDescription { get; set; } = "";
    public decimal Price  { get; set; }
    public int Quantity { get; set; }

    public int TransactionId { get; set; }
    public Transaction Transaction { get; set; }

}
