using FluentValidation;
using Kalendario.Application.Queries;

namespace Kalendario.Application.Validators
{
    public class GetCustomersRequestValidator : AbstractValidator<GetEmployeesRequest>
    {
        public GetCustomersRequestValidator()
        {
            RuleFor(r => r.Search).MaximumLength(120).NotEmpty();
            RuleFor(r => r.Length).LessThanOrEqualTo(150).NotEmpty();
        }
    }
}