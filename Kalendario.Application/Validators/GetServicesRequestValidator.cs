using FluentValidation;
using Kalendario.Application.Queries;

namespace Kalendario.Application.Validators
{
    public class GetServicesRequestValidator : AbstractValidator<GetEmployeesRequest>
    {
        public GetServicesRequestValidator()
        {
            RuleFor(r => r.Search).MaximumLength(120).NotEmpty();
            RuleFor(r => r.Length).LessThanOrEqualTo(150).NotEmpty();
        }
    }
}