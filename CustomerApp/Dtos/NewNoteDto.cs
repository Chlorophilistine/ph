namespace CustomerApp.Dtos
{
    using System.Runtime.Serialization;

    [DataContract(Name = "Note")]
    public class NewNoteDto
    {
        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public string Content { get; set; }
    }
}