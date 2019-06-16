namespace CustomerApp.Controllers.APIv1
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using DataAccess.Models;
    using DataAccess.Repositories;
    using Dtos;

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CustomersController : ApiController
    {
        private readonly ICustomerRepository _customerRepository;

        [ImportingConstructor]
        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        [Route(CustomerRoutes.Customers)]
        public IEnumerable<CustomerSummaryDto> GetCustomers()
        {
            return _customerRepository
                .GetCustomerSummaries()
                .Select(cs => cs.ToDto());
        }

        [HttpGet]
        [Route(CustomerRoutes.Customer)]
        [ResponseType(typeof(CustomerDetailDto))]
        public async Task<IHttpActionResult> GetCustomer(int customerId)
        {
            var (result, customer) = await _customerRepository.GetCustomerDetail(customerId);

            if (result is Result.NotFound)
            {
                return NotFound();
            }

            return Ok(customer.ToDto());
        }

        [HttpGet]
        [Route(CustomerRoutes.CustomerNotes)]
        public IEnumerable<NoteDetailDto> GetNotes(int customerId)
        {
            return _customerRepository
                .GetCustomerNotes(customerId)
                .Select(nd => nd.ToDto());
        }

        [HttpPut]
        [Route(CustomerRoutes.StatusUpdate)]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> CustomerStatusUpdate(StatusUpdateDto statusUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (isParsed, status) = statusUpdateDto.Status.TryParse<Status>();
            if (!isParsed)
            {
                return BadRequest("unrecognised status");
            }

            var result = await _customerRepository.UpdateCustomerStatus(statusUpdateDto.CustomerId, status);

            if (result is Result.Completed) return StatusCode(HttpStatusCode.NoContent);

            return NotFound();
        }

        [HttpPost]
        [ResponseType(typeof(CustomerDetailDto))]
        [Route(CustomerRoutes.Customer)]
        public async Task<IHttpActionResult> PostCustomer(CustomerDetailDto customerDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (isParsed, _) = customerDetailDto.Status.TryParse<Status>();
            if (!isParsed)
            {
                return BadRequest("unrecognised status");
            }

            var newCustomerDetail = await _customerRepository.AddCustomer(customerDetailDto.ToModel());

            return CreatedAtRoute("DefaultApi", new { id = newCustomerDetail.Id }, newCustomerDetail.ToDto());
        }

        [HttpDelete]
        [ResponseType(typeof(CustomerDetailDto))]
        [Route(CustomerRoutes.Customer)]
        public async Task<IHttpActionResult> DeleteCustomer(int customerId)
        {
            var (result, customer) = await _customerRepository.DeleteCustomer(customerId);

            if (result == Result.Completed)
            {
                return Ok(customer.ToDto());
            }

            return NotFound();
        }
    }
}