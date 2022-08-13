using Customer.Api.Controllers;
using Customer.Api.Features.Customer.Data;
using Customer.Api.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xunit;

namespace Customer.Api.Unit.Tests;

[Trait("Category","Customer")]
public class GetCustomerTests
{
    [Fact]
    public async Task Should_Return_Existing_Customer()
    {
        // Arrange
        const int customerId = 1;
        
        var customerQueries = A.Fake<ICustomerQueries>();

        var customerDataModel = new CustomerDataModel
        {
            Id = customerId,
            Name = "a test customer"
        };

        A.CallTo(() => customerQueries.Get(customerId)).Returns(customerDataModel);
        
        var controller = new CustomerController(
            A.Fake<ILogger<CustomerController>>(),
            A.Fake<ICustomerCommands>(),
            customerQueries);
        
        // Act
        var actionResult = await controller.Get(customerId);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

        var actualModel = Assert.IsType<CustomerModel>(okResult.Value);
        
        Assert.Equal(customerDataModel.Id, actualModel.Id);
        Assert.Equal(customerDataModel.Name, actualModel.Name);
        
        A.CallTo(() => customerQueries.Get(customerId)).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task Should_Return_Not_Found_For_Unknown_Customer_Id()
    {
        // Arrange
        const int unknownCustomerId = 99999;
        
        var customerQueries = A.Fake<ICustomerQueries>();

        A.CallTo(() => customerQueries.Get(unknownCustomerId)).Returns((CustomerDataModel)null);
        
        var controller = new CustomerController(
            A.Fake<ILogger<CustomerController>>(),
            A.Fake<ICustomerCommands>(),
            customerQueries);
        
        // Act
        var actionResult = await controller.Get(unknownCustomerId);
        
        // Assert
        Assert.IsType<NotFoundResult>(actionResult.Result);
        
        A.CallTo(() => customerQueries.Get(unknownCustomerId)).MustHaveHappenedOnceExactly();
    }
}