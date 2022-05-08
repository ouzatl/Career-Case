using Career.Contract.Contracts.Job;
using FluentValidation;

namespace FluentValidationDemo.Validation
{
    public class JobContractValidator : AbstractValidator<JobContract>
    {
        public JobContractValidator()
        {
            RuleFor(job => job).NotNull().WithMessage("Please Send Correct Req");
            RuleFor(job => job.CompanyId).NotNull().GreaterThan(0).WithMessage("Please Add Company");
            RuleFor(job => job.Position).NotNull().NotEmpty().WithMessage("Please Add Position");
            RuleFor(job => job.Description).NotNull().NotEmpty().WithMessage("Please Add Description");
            RuleFor(job => job.AvailableFrom).NotNull().GreaterThanOrEqualTo(DateTime.Now).WithMessage("Please Add Correct Date");
        }
    }
}