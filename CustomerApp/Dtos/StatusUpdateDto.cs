namespace CustomerApp.Dtos
{
    using System.Runtime.Serialization;

    [DataContract(Name = "StatusUpdate")]
    public class StatusUpdateDto
    {
        [DataMember]
        public int CustomerId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Status { get; set; }
    }
}