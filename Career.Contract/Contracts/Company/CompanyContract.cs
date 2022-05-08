using ProtoBuf;

namespace Career.Contract.Contracts.Company
{
    [ProtoContract]
    public class CompanyContract
    {
        [ProtoMember(1)]
        public string PhoneNumber { get; set; }
        [ProtoMember(2)]
        public string Address { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
    }
}