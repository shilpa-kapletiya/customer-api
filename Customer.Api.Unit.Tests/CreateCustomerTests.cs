using Customer.Api.Controllers;
using Customer.Api.Features.Customer.Data;
using Customer.Api.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Customer.Api.Unit.Tests;

[Trait("Category","Customer")]
public class CreateCustomerTests
{
    [Fact]
    public async Task Should_Create_New_Customer()
    {
        // Arrange
        var customerCommands = A.Fake<ICustomerCommands>();

        const int id = 1;
        const string name = "new customer name";
        
        var customerCreateModel = new CustomerCreateModel
        {
            Name = name
        };

        A.CallTo(() => customerCommands.Create(
            A<CustomerCreateDataModel>.That.Matches(c => c.Name == name)))
            .Returns(id);
        
        var controller = new CustomerController(
            A.Fake<ILogger<CustomerController>>(),
            customerCommands,
            A.Fake<ICustomerQueries>());
        
        // Act
        var actionResult = await controller.Post(customerCreateModel);
        
        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult);

        var actualModel = Assert.IsType<CustomerModel>(createdResult.Value);

        Assert.Equal(id, actualModel.Id);
        Assert.Equal(name, actualModel.Name);
        
        A.CallTo(() => customerCommands.Create(A<CustomerCreateDataModel>.That.Matches(c => c.Name == name)))
            .MustHaveHappenedOnceExactly();
    }
}