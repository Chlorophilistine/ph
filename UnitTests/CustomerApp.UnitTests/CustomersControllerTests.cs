namespace CustomerApp.UnitTests
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http.Results;
    using Controllers.APIv1;
    using DataAccess.Models;
    using DataAccess.Repositories;
    using Dtos;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class CustomersControllerTests
    {
        [TestCase(1)]
        [TestCase(23)]
        public async Task WhenGetCustomer_AndCustomerNotFound_ThenNotFoundReturned(int customerId)
        {
            var emptyRepo = new Mock<ICustomerRepository>(MockBehavior.Strict);
            emptyRepo
                .Setup(cr => cr.GetCustomerDetail(It.Is<int>(id => id == customerId)))
                .Returns(Task.FromResult((Result.NotFound, (CustomerDetail)null)));

            var sut = new CustomersController(emptyRepo.Object);

            var result = await sut.GetCustomer(customerId);

            result.Should().BeAssignableTo<NotFoundResult>();
        }

        [TestCase(1)]
        [TestCase(23)]
        public async Task WhenCustomerFound_ThenOkResultReturned(int customerId)
        {
            var occuppiedRepo = new Mock<ICustomerRepository>(MockBehavior.Strict);
            occuppiedRepo
                .Setup(cr => cr.GetCustomerDetail(It.Is<int>(id => id == customerId)))
                .Returns((int id) => Task.FromResult((Result.Completed, new CustomerDetail {Id = id})));

            var sut = new CustomersController(occuppiedRepo.Object);

            var result = await sut.GetCustomer(customerId);

            result.Should().BeAssignableTo<OkNegotiatedContentResult<CustomerDetailDto>>();

            var contentResult = result as OkNegotiatedContentResult<CustomerDetailDto>;

            contentResult.Content.Should().NotBeNull();
            contentResult.Content.Id.Should().Be(customerId);
        }

        [TestCase(1, "mauve")]
        [TestCase(23, "pensive")]
        public async Task WhenUpdateCustomer_AndBadStatusProvided_ThenBadRequestReturned(int customerId, string notAStatus)
        {
            notAStatus.TryParse<Status>().parsed.Should().BeFalse();

            var repo = new Mock<ICustomerRepository>(MockBehavior.Strict);

            var sut = new CustomersController(repo.Object);

            var result = await sut.CustomerStatusUpdate(new StatusUpdateDto
            {
                CustomerId = customerId, Status = notAStatus
            });

            result.Should().BeAssignableTo<BadRequestErrorMessageResult>();
            ((BadRequestErrorMessageResult) result).Message.Should().Be("unrecognised status");
        }

        [TestCase(1, Status.Current)]
        [TestCase(23, Status.Prospective)]
        public async Task WhenUpdateCustomer_AndCustomerNotFound_ThenNotFoundReturned(int customerId, Status anyStatus)
        {
            var emptyRepo = new Mock<ICustomerRepository>(MockBehavior.Strict);
            emptyRepo
                .Setup(cr => cr.UpdateCustomerStatus(It.Is<int>(id => id == customerId), It.IsAny<Status>()))
                .Returns(Task.FromResult(Result.NotFound));

            var sut = new CustomersController(emptyRepo.Object);

            var result = await sut.CustomerStatusUpdate(new StatusUpdateDto {CustomerId = customerId, Status = anyStatus.ToString()});

            result.Should().BeAssignableTo<NotFoundResult>();
        }

        [TestCase(1, Status.Current)]
        [TestCase(23, Status.Prospective)]
        public async Task WhenUpdateCustomer_AndCustomerFound_Then204Returned(int customerId, Status anyStatus)
        {
            var repo = new Mock<ICustomerRepository>(MockBehavior.Strict);
            repo
                .Setup(cr => cr.UpdateCustomerStatus(It.Is<int>(id => id == customerId), It.Is<Status>(s => s == anyStatus)))
                .Returns(Task.FromResult(Result.Completed));

            var sut = new CustomersController(repo.Object);

            var result = await sut.CustomerStatusUpdate(new StatusUpdateDto { CustomerId = customerId, Status = anyStatus.ToString() });

            result.Should().BeAssignableTo<StatusCodeResult>();
            ((StatusCodeResult) result).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [TestCase(1, Status.NonActive)]
        [TestCase(23, Status.Current)]
        public async Task WhenPostNewCustomer_AndSucceeds_ThenOkReturned(int newCustomerId, Status newStatus)
        {
            var repo = new Mock<ICustomerRepository>(MockBehavior.Strict);
            repo
                .Setup(cr => cr.AddCustomer(It.IsAny<CustomerDetail>()))
                .Returns(Task.FromResult(new CustomerDetail{Id = newCustomerId}));

            var sut = new CustomersController(repo.Object);

            var newCustomerDto = new CustomerDetailDto
            {
                Status = newStatus.ToString(), Created = DateTime.Now.ToString("O", CultureInfo.InvariantCulture)
            };

            var result = await sut.PostCustomer(newCustomerDto);

            result.Should().BeAssignableTo<CreatedAtRouteNegotiatedContentResult<CustomerDetailDto>>();

            var newCustomerResult = (CreatedAtRouteNegotiatedContentResult<CustomerDetailDto>)result;
            newCustomerResult.RouteName.Should().Be("DefaultApi");
            newCustomerResult.RouteValues["id"].Should().Be(newCustomerId);
        }

        [TestCase("Peach")]
        [TestCase("Castigated")]
        public async Task WhenPostNewCustomer_AndBadStatusProvided_ThenBadRequestReturned(string badStatus)
        {
            var repo = new Mock<ICustomerRepository>(MockBehavior.Strict);
            var sut = new CustomersController(repo.Object);

            var newCustomerWithUnknownStatus = new CustomerDetailDto
            {
                Status = badStatus, Created = DateTime.Now.ToString("O", CultureInfo.InvariantCulture)
            };

            var result = await sut.PostCustomer(newCustomerWithUnknownStatus);

            result.Should().BeAssignableTo<BadRequestErrorMessageResult>();
            ((BadRequestErrorMessageResult)result).Message.Should().Be("unrecognised status");
        }

        [TestCase(936)]
        [TestCase(57)]
        public async Task WhenDeleteCustomer_AndCustomerFound_ThenOkReturned(int customerId)
        {
            var repo = new Mock<ICustomerRepository>(MockBehavior.Strict);
            repo
                .Setup(cr => cr.DeleteCustomer(It.Is<int>(id => id == customerId)))
                .Returns(Task.FromResult((Result.Completed, new CustomerDetail {Id = customerId})));

            var sut = new CustomersController(repo.Object);

            var result = await sut.DeleteCustomer(customerId);

            result.Should().BeAssignableTo<OkNegotiatedContentResult<CustomerDetailDto>>();

            var contentResult = result as OkNegotiatedContentResult<CustomerDetailDto>;

            contentResult.Content.Should().NotBeNull();
            contentResult.Content.Id.Should().Be(customerId);
        }

        [TestCase(936)]
        [TestCase(57)]
        public async Task WhenDeleteCustomer_AndCustomerNotFound_ThenNotFoundReturned(int customerId)
        {
            var emptyRepo = new Mock<ICustomerRepository>(MockBehavior.Strict);
            emptyRepo
                .Setup(cr => cr.DeleteCustomer(It.Is<int>(id => id == customerId)))
                .Returns(Task.FromResult((Result.NotFound, (CustomerDetail)null)));

            var sut = new CustomersController(emptyRepo.Object);

            var result = await sut.DeleteCustomer(customerId);

            result.Should().BeAssignableTo<NotFoundResult>();
        }
    }
}