namespace CustomerApp.DataAccess.Models
{
    public class CustomerSummary
    {
        public int Id { get; set; }

        public Status Status { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }
    }
}