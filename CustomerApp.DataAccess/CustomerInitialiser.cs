namespace CustomerApp.DataAccess
{
    using System;
    using System.Data.Entity;
    using Entities;
    using Models;

    public class CustomerInitialiser : DropCreateDatabaseIfModelChanges<CustomerContext>
    {
        protected override void Seed(CustomerContext context)
        {
            // add customers
            context.Customers.Add(new Customer
            {
                Id = 1,
                Status = Status.Prospective,
                Created = new DateTime(2019, 2, 12),
                FirstName = "Joe",
                LastName = "Bloggs",
                Company = "Amalgamated Holdings",
                Email = "joe@bloggs.com",
                Address = "1 High Street, Edinburgh"
            });

            context.Customers.Add(new Customer
            {
                Id = 2,
                Status = Status.Current,
                Created = new DateTime(2010, 12, 12),
                FirstName = "Andrea",
                LastName = "Smith",
                Company = "SoleTrader LLP",
                Email = "a@smith.com",
                Address = "456 High Street, Edinburgh"
            });

            context.Customers.Add(new Customer
            {
                Id = 3,
                Status = Status.NonActive,
                Created = new DateTime(1999, 1, 1),
                FirstName = "Zachry",
                LastName = "Twist",
                Company = "Twist Incorporated",
                Email = "z@twist.com",
                Address = "29 Acacia Road, Edinburgh"
            });

            context.SaveChanges();

            // add notes
            context.Notes.Add(new Note { Id = 1, CustomerId = 1, Content = "My first note"});
            context.Notes.Add(new Note { Id = 2, CustomerId = 1, Content = "Prospective customer, follow up on lead" });
            context.Notes.Add(new Note { Id = 3, CustomerId = 2, Content = "Current customer, due quarterly sales review" });
            context.Notes.Add(new Note { Id = 4, CustomerId = 2, Content = "For review" });
            context.Notes.Add(new Note { Id = 5, CustomerId = 2, Content = "Sales order follow up" });

            context.SaveChanges();
        }
    }
}