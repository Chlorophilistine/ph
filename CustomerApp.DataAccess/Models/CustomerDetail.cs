namespace CustomerApp.DataAccess.Models
{
    using System;

    public class CustomerDetail
    {
        public int Id { get; set; }
        public Status Status { get; set; }
        public DateTime Created { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}