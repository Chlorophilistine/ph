namespace CustomerApp.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using DataAccess;
    using DataAccess.Entities;
    using DataAccess.Models;
    using Dtos;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class DtoMapperTests
    {
        [TestCase(1, "some content")]
        [TestCase(5, "some other content")]
        [TestCase(12, null)]
        public void GiveNoteDetail_ThenNoteDetailDto_ContainsExpectedProperties(int id, string content)
        {
            var noteDetail = new NoteDetail{ Id = id, Content = content };

            var noteDetailDto = noteDetail.ToDto();

            noteDetailDto.Id.Should().Be(noteDetail.Id);
            noteDetailDto.Content.Should().Be(noteDetail.Content);
        }

        [TestCase(1, "some content")]
        [TestCase(5, "some other content")]
        [TestCase(12, null)]
        public void GiveNoteDetailDto_ThenNoteDetail_ContainsExpectedProperties(int id, string content)
        {
            var noteDetailDto = new NoteDetailDto { Id = id, Content = content };

            var noteDetail = noteDetailDto.ToModel();

            noteDetail.Id.Should().Be(noteDetail.Id);
            noteDetail.Content.Should().Be(noteDetail.Content);
        }

        [TestCase(1, "some content")]
        [TestCase(5, "some other content")]
        [TestCase(12, null)]
        public void GiveNewNoteDto_ThenNewNote_ContainsExpectedProperties(int id, string content)
        {
            var newNoteDto = new NewNoteDto { CustomerId = id, Content = content };

            var newNote = newNoteDto.ToModel();

            newNote.CustomerId.Should().Be(newNote.CustomerId);
            newNote.Content.Should().Be(newNote.Content);
        }

        [TestCase(1, "Jean", 1999)]
        [TestCase(5, "some other name", 1876)]
        [TestCase(12, null, 1956)]
        public void GivenCustomerDetail_ThenCustomerDetailDto_ContainsExpectedProperties(int id, string firstName, int year)
        {
            var customerDetail = new CustomerDetail
            {
                Id = id,
                Address = "Some Street",
                Created = new DateTime(year, 1, 1),
                FirstName = firstName,
                LastName = "Smith",
                Status = Status.Current,
                Email = $"{firstName}@smith.com"
            };

            var customerDetailsDto = customerDetail.ToDto();

            customerDetailsDto.Id.Should().Be(customerDetail.Id);
            customerDetailsDto.Address.Should().Be(customerDetail.Address);
            customerDetailsDto.Created.Should().Be(customerDetail.Created.ToString("O", CultureInfo.InvariantCulture));
            customerDetailsDto.FirstName.Should().Be(customerDetail.FirstName);
            customerDetailsDto.LastName.Should().Be(customerDetail.LastName);
            customerDetailsDto.Status.Should().Be(customerDetail.Status.ToString());
            customerDetailsDto.Email.Should().Be(customerDetail.Email);
        }

        [TestCase(1, "Jean", 1999)]
        [TestCase(5, "some other name", 1876)]
        [TestCase(12, null, 1956)]
        public void GivenCustomerDetailsDto_ThenCustomerDetails_ContainsExpectedProperties(int id, string firstName, int year)
        {
            var creationDate = new DateTime(year, 1, 1);
            const Status status = Status.Current;

            var customer = new CustomerDetailDto
            {
                Id = id,
                Address = "Some Street",
                Created = creationDate.ToString("O", CultureInfo.InvariantCulture),
                FirstName = firstName,
                LastName = "Smith",
                Status = status.ToString(),
                Email = $"{firstName}@smith.com"
            };

            var customerDetails = customer.ToModel();

            customerDetails.Id.Should().Be(customer.Id);
            customerDetails.Address.Should().Be(customer.Address);
            customerDetails.Created.Should().Be(creationDate);
            customerDetails.FirstName.Should().Be(customer.FirstName);
            customerDetails.LastName.Should().Be(customer.LastName);
            customerDetails.Status.Should().Be(status);
            customerDetails.Email.Should().Be(customer.Email);
        }

        [Test]
        public void WhenMappingDtoToModel_AndCreationDateCannotBeParsed_ThenExceptionThrown()
        {
            var customer = new CustomerDetailDto
            {
                Id = 1,
                Address = "Some Street",
                Created = "not a date",
                FirstName = "Liz",
                LastName = "Smith",
                Status = Status.Current.ToString(),
                Email = $"liz@smith.com"
            };

            Action sut = () => customer.ToModel();

            sut.Should().Throw<FormatException>();
        }

        [Test]
        public void WhenMappingDtoToModel_AndStatusCannotBeParsed_ThenStatusIsProspective()
        {
            const string notAValidStatusString = "not a valid Status string";

            var customer = new CustomerDetailDto
            {
                Id = 1,
                Address = "Some Street",
                Created = new DateTime(2010, 1, 1).ToString("O", CultureInfo.InvariantCulture),
                FirstName = "Liz",
                LastName = "Smith",
                Status = notAValidStatusString,
                Email = "liz@smith.com"
            };

            customer.ToModel().Status.Should().Be(Status.Prospective);
        }

        [TestCase(1, "Jean")]
        [TestCase(5, "some other name")]
        [TestCase(12, null)]
        public void GivenCustomerSummary_ThenCustomerSummaryDto_ContainsExpectedProperties(int id, string firstName)
        {
            var customerSummary = new CustomerSummary
            {
                Id = id,
                FirstName = firstName,
                LastName = "Smith",
                Status = Status.Current,
            };

            var customerSummaryDto = customerSummary.ToDto();

            customerSummaryDto.Id.Should().Be(customerSummary.Id);
            customerSummaryDto.FirstName.Should().Be(customerSummary.FirstName);
            customerSummaryDto.LastName.Should().Be(customerSummary.LastName);
            customerSummaryDto.Status.Should().Be(customerSummary.Status.ToString());
        }

        [TestCase(1, "Jean", 1999)]
        [TestCase(5, "some other name", 1876)]
        [TestCase(12, null, 1956)]
        public void GivenCustomerWithNotes_ThenCustomerDetails_ContainsExpectedProperties(int id, string firstName, int year)
        {
            var customer = new Customer
            {
                Id = id,
                Address = "Some Street",
                Created = new DateTime(year, 1, 1),
                FirstName = firstName,
                LastName = "Smith",
                Status = Status.Current,
                Email = "liz@smith.com",
                Notes = new List<Note>
                {
                    new Note {Id = 1, Content = "Some note"},
                    new Note {Id = 2, Content = "2nd note"},
                    new Note {Id = 3, Content = "3rd note"},
                    new Note {Id = 4, Content = "4th note"}
                }
            };

            var customerDetails = customer.ToModel();

            customerDetails.Id.Should().Be(customer.Id);
            customerDetails.Address.Should().Be(customer.Address);
            customerDetails.Created.Should().Be(customer.Created);
            customerDetails.FirstName.Should().Be(customer.FirstName);
            customerDetails.LastName.Should().Be(customer.LastName);
            customerDetails.Status.Should().Be(customer.Status);
            customerDetails.Email.Should().Be(customer.Email);
        }
    }
}