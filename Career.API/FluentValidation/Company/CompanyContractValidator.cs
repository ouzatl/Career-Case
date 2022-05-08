using Career.Contract.Contracts.Company;
using FluentValidation;

namespace FluentValidationDemo.Validation
{
    public class CompanyContractValidator : AbstractValidator<CompanyContract>
    {
        public CompanyContractValidator()
        {
            RuleFor(company => company).NotNull().WithMessage("Please Send Correct Req");
            RuleFor(company => company.PhoneNumber).NotNull().NotEmpty().WithMessage("Please Add Correct Phone Number");
            RuleFor(company => company.Address).NotNull().NotEmpty().WithMessage("Please Add Address");
            RuleFor(company => company.Name).NotNull().NotEmpty().WithMessage("Please Add Name");
        }
    }
}