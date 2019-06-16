namespace CustomerApp.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Models;

    [Export(typeof(ICustomerRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ICustomerContext _context;

        [ImportingConstructor]
        public CustomerRepository(ICustomerContext context)
        {
            _context = context;
        }

        public IEnumerable<CustomerSummary> GetCustomerSummaries()
        {
            return _context.Customers
                .Select(Mapper.AsCustomerSummary)
                .ToArray();
        }

        public async Task<(Result, CustomerDetail)> GetCustomerDetail(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            return customer is null
                ? (Result.NotFound, null)
                : (Result.Completed, customer.ToModel());
        }

        public async Task<Result> UpdateCustomerStatus(int customerId, Status newStatus)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer is null)
            {
                return Result.NotFound;
            }

            customer.Status = newStatus;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customerId))
                {
                    return Result.NotFound;
                }

                throw;
            }

            return Result.Completed;
        }

        public async Task<CustomerDetail> AddCustomer(CustomerDetail customerDetail)
        {
            var customer = customerDetail.ToEntity();
            customer.Created = DateTime.Now;

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return customer.ToModel();
        }

        public async Task<(Result, CustomerDetail)> DeleteCustomer(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
            {
                return (Result.NotFound, null);
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return (Result.Completed, customer.ToModel());
        }

        public IEnumerable<NoteDetail> GetCustomerNotes(int customerId)
        {
            return _context.Notes
                .Where(n => n.CustomerId == customerId)
                .Select(Mapper.ToNoteDetails)
                .ToArray();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Count(e => e.Id == id) > 0;
        }
    }
}