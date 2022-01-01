using FluentValidation;
using Kalendario.Application.Queries.Admin;

namespace Kalendario.Application.Validators;

public class GetServicesRequestValidator : AbstractValidator<GetEmployeesQuery>
{
    public GetServicesRequestValidator()
    {
        RuleFor(r => r.Search).MaximumLength(120);
        RuleFor(r => r.Length).LessThanOrEqualTo(150).NotEmpty();
    }
}