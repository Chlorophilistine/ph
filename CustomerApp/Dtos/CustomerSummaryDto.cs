namespace CustomerApp.Dtos
{
    using System.Runtime.Serialization;

    [DataContract(Name = "CustomerSummary")]
    public class CustomerSummaryDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Status { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string LastName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FirstName { get; set; }
    }
}