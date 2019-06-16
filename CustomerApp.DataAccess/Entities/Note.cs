namespace CustomerApp.DataAccess.Entities
{
    public class Note
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}