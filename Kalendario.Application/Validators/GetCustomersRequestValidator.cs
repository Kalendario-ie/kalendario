using FluentValidation;
using Kalendario.Application.Queries.Admin;

namespace Kalendario.Application.Validators;

public class GetCustomersRequestValidator : AbstractValidator<GetEmployeesRequest>
{
    public GetCustomersRequestValidator()
    {
        RuleFor(r => r.Search).MaximumLength(120);
        RuleFor(r => r.Length).LessThanOrEqualTo(150).NotEmpty();
    }
}