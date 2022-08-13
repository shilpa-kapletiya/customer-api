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
public class DeleteCustomerTests
{
    [Fact]
    public async Task Should_Delete_Customer()
    {
        // Arrange
        var customerCommands = A.Fake<ICustomerCommands>();

        const int id = 1;

        var controller = new CustomerController(
            A.Fake<ILogger<CustomerController>>(),
            customerCommands,
            A.Fake<ICustomerQueries>());
        
        // Act
        var actionResult = await controller.Delete(id);
        
        // Assert
        Assert.IsType<NoContentResult>(actionResult);

        A.CallTo(() =>
                customerCommands.Delete(id))
            .MustHaveHappenedOnceExactly();
    }
}