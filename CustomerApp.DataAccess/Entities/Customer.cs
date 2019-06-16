namespace CustomerApp.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using Models;

    public class Customer
    {
        public int Id { get; set; }
        public Status Status { get; set; }
        public DateTime Created { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
    }
}