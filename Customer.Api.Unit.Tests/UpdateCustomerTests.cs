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
public class UpdateCustomerTests
{
    [Fact]
    public async Task Should_Update_Customer()
    {
        // Arrange
        var customerCommands = A.Fake<ICustomerCommands>();

        const int id = 1;
        const string updatedName = "updated customer name";
        
        var customerUpdateModel = new CustomerUpdateModel()
        {
            Name = updatedName
        };
        
        var controller = new CustomerController(
            A.Fake<ILogger<CustomerController>>(),
            customerCommands,
            A.Fake<ICustomerQueries>());
        
        // Act
        var actionResult = await controller.Put(id, customerUpdateModel);
        
        // Assert
        Assert.IsType<NoContentResult>(actionResult);

        A.CallTo(() =>
            customerCommands.Update(id, A<CustomerUpdateDataModel>.That.Matches(c => c.Name == updatedName)))
            .MustHaveHappenedOnceExactly();
    }
}