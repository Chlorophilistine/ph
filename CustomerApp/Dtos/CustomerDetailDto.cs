namespace CustomerApp.Dtos
{
    using System.Runtime.Serialization;

    [DataContract(Name="CustomerDetail")]
    public class CustomerDetailDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Status { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Created { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string LastName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FirstName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Email { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Address { get; set; }
    }
}