using AutoMapper;
using Career.Contract.Contracts.Company;
using Career.Contract.Contracts.Job;
using Career.Data.Models;

namespace Career.Data.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Company, CompanyContract>().ReverseMap()
            .ForMember(a => a.IsActive, opt => opt.Ignore());

            CreateMap<Job, JobContract>().ReverseMap();
        }
    }
}