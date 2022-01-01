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
            .Matches("^[!@$%^&*(){}:;<>,.?/+_=|'~\\-“\"]*$")
            .WithMessage("'{PropertyName}' must not contain the following characters £ # “” or spaces.");
    }
}