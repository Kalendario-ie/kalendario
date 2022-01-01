using FluentValidation;
using Kalendario.Application.Commands.Admin;

namespace Kalendario.Application.Validators;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(c => c.Name)
            .MinimumLength(10)
            .MaximumLength(120)
            .NotNull()
            .NotEmpty()
            .Matches(@"\A[A-Za-z0-9\x20]*\Z")
            .WithMessage("'{PropertyName}' may only contain characters numbers and space.");
    }
}