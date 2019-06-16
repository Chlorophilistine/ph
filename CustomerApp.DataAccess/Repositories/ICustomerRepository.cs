namespace CustomerApp.DataAccess.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface ICustomerRepository
    {
        IEnumerable<CustomerSummary> GetCustomerSummaries();
        Task<(Result result, CustomerDetail customer)> GetCustomerDetail(int customerId);
        Task<Result> UpdateCustomerStatus(int customerId, Status newStatus);
        Task<CustomerDetail> AddCustomer(CustomerDetail customerDetail);
        Task<(Result result, CustomerDetail customer)> DeleteCustomer(int customerId);
        IEnumerable<NoteDetail> GetCustomerNotes(int customerId);
    }
}