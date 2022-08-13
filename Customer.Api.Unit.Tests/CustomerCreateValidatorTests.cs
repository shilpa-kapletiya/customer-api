using Customer.Api.Features.Customer.Validators;
using Customer.Api.Models;
using FluentValidation.TestHelper;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Customer.Api.Unit.Tests;

[Trait("Category","Customer")]
public class CustomerCreateValidatorTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task Should_Error_When_Name_Is_Null_Or_Empty(string name)
    {
        // Arrange
        var validator = new CustomerCreateModelValidator();

        var customerCreateModel = new CustomerCreateModel
        {
            Name = name
        };
        
        // Act
        var result = await validator.TestValidateAsync(customerCreateModel);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Name must not be empty");
    }
    
    [Theory]
    [InlineData(18)]
    [InlineData(17)]
    public async Task Should_Error_When_Age_Is_Invalid(int age)
    {
        // Arrange
        var validator = new CustomerCreateModelValidator();

        var customerCreateModel = new CustomerCreateModel
        {
            Name = "some name",
            DateOfBirth = DateTime.Today.AddYears(-age)
        };
        
        // Act
        var result = await validator.TestValidateAsync(customerCreateModel);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.DateOfBirth)
            .WithErrorMessage("Must be over 18");
    }
    
    [Fact]
    public async Task Should_Not_Error_Name()
    {
        // Arrange
        var validator = new CustomerCreateModelValidator();

        var customerCreateModel = new CustomerCreateModel
        {
            Name = "some name",
            DateOfBirth = DateTime.Now.AddYears(-19)
        };
        
        // Act
        var result = await validator.TestValidateAsync(customerCreateModel);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}