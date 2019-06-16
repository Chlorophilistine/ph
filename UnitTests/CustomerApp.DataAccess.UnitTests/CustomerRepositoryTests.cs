namespace CustomerApp.DataAccess.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using FluentAssertions;
    using Models;
    using Moq;
    using NUnit.Framework;
    using Repositories;

    [TestFixture]
    public class CustomerRepositoryTests
    {
        [Test]
        public async Task WhenNewCustomerAdded_ThenRepositoryDeterminesCreationTime()
        {
            var customers = new List<Customer>();
            var mockCustomerSet = new Mock<DbSet<Customer>>().SetupData(customers);
            var mockContext = new Mock<ICustomerContext>();
            mockContext
                .Setup(c => c.Customers)
                .Returns(mockCustomerSet.Object);

            var sut = new CustomerRepository(mockContext.Object);

            var newCustomerDetail = new CustomerDetail
            {
                Created = DateTime.MinValue
            };
            var newCustomer = await sut.AddCustomer(newCustomerDetail);

            newCustomer.Created.Should().NotBe(DateTime.MinValue);
            newCustomer.Created
                .Should()
                .BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(10), "Repo applies the creation timestamp");

            customers.Single().Created.Should().Be(newCustomer.Created);
        }

        [TestCase(1, Status.Current)]
        [TestCase(4, Status.Prospective)]
        public async Task WhenCustomerUpdated_AndCustomerNotFound_ThenResultIsNotFound(int customerId, Status newStatus)
        {
            var noCustomers = new List<Customer>();
            var emptyCustomerSet = new Mock<DbSet<Customer>>().SetupData(noCustomers);
            var mockContext = new Mock<ICustomerContext>();
            mockContext
                .Setup(c => c.Customers)
                .Returns(emptyCustomerSet.Object);

            var sut = new CustomerRepository(mockContext.Object);

            var result = await sut.UpdateCustomerStatus(customerId, newStatus);

            result.Should().Be(Result.NotFound);
        }

        [TestCase(1)]
        [TestCase(4)]
        public async Task WhenCustomerDeleted_AndCustomerNotFound_ThenResultIsNotFound(int customerId)
        {
            var noCustomers = new List<Customer>();
            var emptyCustomerSet = new Mock<DbSet<Customer>>().SetupData(noCustomers);
            var mockContext = new Mock<ICustomerContext>();
            mockContext
                .Setup(c => c.Customers)
                .Returns(emptyCustomerSet.Object);

            var sut = new CustomerRepository(mockContext.Object);

            var (result, _) = await sut.DeleteCustomer(customerId);

            result.Should().Be(Result.NotFound);
        }
    }
}