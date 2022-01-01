using FluentValidation;
using Kalendario.Application.Queries.Admin;

namespace Kalendario.Application.Validators;

public class GetEmployeesQueryValidator : AbstractValidator<GetEmployeesQuery>
{
    public GetEmployeesQueryValidator()
    {
        RuleFor(r => r.Search).MaximumLength(120);
        RuleFor(r => r.Length).LessThanOrEqualTo(150).NotEmpty();
    }
}