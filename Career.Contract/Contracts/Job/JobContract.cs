using ProtoBuf;

namespace Career.Contract.Contracts.Job
{
    [ProtoContract]
    public class JobContract
    {
        [ProtoMember(1)]
        public int CompanyId { get; set; }
        [ProtoMember(2)]
        public string Position { get; set; }
        [ProtoMember(3)]
        public string Description { get; set; }
        [ProtoMember(4)]
        public DateTime AvailableFrom { get; set; }
        [ProtoMember(5)]
        public string Benefits { get; set; }
        [ProtoMember(6)]
        public string TypeOfWork { get; set; }
        [ProtoMember(7)]
        public double Salary { get; set; }
    }
}