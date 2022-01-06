using System;
using FluentValidation;
using Kalendario.Application.Commands.Admin;

namespace Kalendario.Application.Validators;

public class UpsertCustomerCommandValidator : AbstractValidator<UpsertCustomerCommand>
{
    public UpsertCustomerCommandValidator()
    {
        RuleFor(r => r.Name).MaximumLength(120);
        RuleFor(r => r.Warning).MaximumLength(255);
        RuleFor(r => r.Email).EmailAddress().MaximumLength(255);
        RuleFor(r => r.PhoneNumber).MaximumLength(255);
    }
}