namespace CustomerApp.Dtos
{
    using System.Runtime.Serialization;

    [DataContract(Name = "NoteDetail")]
    public class NoteDetailDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Content { get; set; }
    }
}