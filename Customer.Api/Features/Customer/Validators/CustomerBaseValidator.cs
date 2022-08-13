using Customer.Api.Models;
using FluentValidation;
using System;

namespace Customer.Api.Features.Customer.Validators;

public abstract class CustomerBaseValidator<T> : AbstractValidator<T> 
    where T : ICustomerCommonModel
{
    protected CustomerBaseValidator()
    {
        this.RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name must not be empty");

        this.RuleFor(x => x.DateOfBirth)
            .Must(x => (DateTime.Now.Year - x.Year) > 18)
            .WithMessage("Must be over 18");
    }
}