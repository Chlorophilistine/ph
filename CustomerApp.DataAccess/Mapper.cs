namespace CustomerApp.DataAccess
{
    using System;
    using System.Linq.Expressions;
    using Entities;
    using Models;

    public static class Mapper
    {
        public static Expression<Func<Customer, CustomerSummary>> AsCustomerSummary =>
            customer => new CustomerSummary
            {
                Id = customer.Id,
                Status = customer.Status,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
            };

        public static Expression<Func<Note, NoteDetail>> ToNoteDetails =>
            note => new NoteDetail
            {
                Id = note.Id,
                Content = note.Content
            };

        public static Note ToEntity(this NewNote newNote) =>
            new Note
            {
                CustomerId = newNote.CustomerId,
                Content = newNote.Content
            };

        public static NoteDetail ToModel(this Note note) =>
            new NoteDetail
            {
                Id = note.Id,
                Content = note.Content
            };

        public static CustomerDetail ToModel(this Customer customer) =>
            new CustomerDetail
            {
                Id = customer.Id,
                Status = customer.Status,
                Created = customer.Created,
                Address = customer.Address,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName
            };

        public static Customer ToEntity(this CustomerDetail customer) =>
            new Customer
            {
                Id = customer.Id,
                Status = customer.Status,
                Created = customer.Created,
                Address = customer.Address,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName
            };
    }
}