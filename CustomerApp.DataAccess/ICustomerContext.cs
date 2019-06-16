namespace CustomerApp.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Entities;

    public interface ICustomerContext : IDisposable
    {
        DbSet<Customer> Customers { get; set; }
        DbSet<Note> Notes { get; set; }

        Task<int> SaveChangesAsync();
    }
}