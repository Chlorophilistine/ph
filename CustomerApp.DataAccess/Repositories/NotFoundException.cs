namespace CustomerApp.DataAccess.Repositories
{
    using System;

    public class NotFoundException : Exception
    {
        public int Id { get; }

        public NotFoundException(int id)
        {
            Id = id;
        }
    }
}