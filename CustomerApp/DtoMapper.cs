namespace CustomerApp
{
    using System;
    using System.Globalization;
    using DataAccess.Models;
    using Dtos;

    public static class DtoMapper
    {
        public static CustomerSummaryDto ToDto(this CustomerSummary cs) =>
            new CustomerSummaryDto
            {
                Id = cs.Id,
                Status = cs.Status.ToString(),
                FirstName = cs.FirstName,
                LastName = cs.LastName,
            };

        public static NoteDetailDto ToDto(this NoteDetail model) =>
            new NoteDetailDto
            {
                Id = model.Id,
                Content = model.Content
            };

        public static CustomerDetailDto ToDto(this CustomerDetail model) =>
            new CustomerDetailDto
            {
                Id = model.Id,
                Status = model.Status.ToString(),
                Created = model.Created.ToString("O", CultureInfo.InvariantCulture),
                Address = model.Address,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

        public static NoteDetail ToModel(this NoteDetailDto dto) =>
            new NoteDetail
            {
                Id = dto.Id,
                Content = dto.Content
            };

        public static NewNote ToModel(this NewNoteDto dto) =>
            new NewNote
            {
                CustomerId = dto.CustomerId,
                Content = dto.Content
            };

        public static CustomerDetail ToModel(this CustomerDetailDto dto) =>
            new CustomerDetail
            {
                Id = dto.Id,
                Status = dto.Status.TryParse<Status>().result,
                Created = DateTime.Parse(dto.Created, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                Address = dto.Address,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };
    }
}