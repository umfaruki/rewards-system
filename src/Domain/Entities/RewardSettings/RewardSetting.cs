namespace Domain.Entities.RewardSettings;

public class RewardSetting
{
    public int Id { get; set; }
    public decimal MinSpentAmount { get; set; }
    public int MinSpentAmountPoints { get; set; }
    public decimal UpperRangeSpentAmount { get; set; }
    public int UpperRangeSpentPoints { get; set; }
    public int TaxRate { get; set; }
}
