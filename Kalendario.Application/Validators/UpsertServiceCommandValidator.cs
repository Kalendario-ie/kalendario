using System;
using FluentValidation;
using Kalendario.Application.Commands.Admin;

namespace Kalendario.Application.Validators;

public class UpsertServiceCommandValidator : AbstractValidator<UpsertServiceCommand>
{
    public UpsertServiceCommandValidator()
    {
        RuleFor(r => r.Name).MaximumLength(120);
        RuleFor(r => r.Description).MaximumLength(255);
        RuleFor(r => r.Price).GreaterThan(0).NotNull();
        RuleFor(r => r.Duration).GreaterThan(TimeSpan.Zero).NotNull();
        RuleFor(r => r.ServiceCategoryId).NotNull().NotEmpty();
    }
}