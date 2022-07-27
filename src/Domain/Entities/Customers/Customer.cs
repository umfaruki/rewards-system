namespace Domain.Entities.Customers
{
    /// <summary>
    /// This is Customer Entity
    /// </summary>
    public class Customer
    {
        public Customer()
        {
            DateJoined = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public string CustomerNumber { get; set; } = "";
        public DateTime DateJoined { get; set; }
        public string Firstname { get; set; } = "";
        public string Surname { get; set; } = "";
        public string Email { get; set; } = "";
        public string Telephone { get; set; } = "";
    }
}
