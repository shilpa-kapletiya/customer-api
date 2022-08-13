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
public class GetCustomersTests
{
    [Fact]
    public async Task Should_Return_Exiting_Customers()
    {
        // Arrange
        const string filter = "";
        
        var customerQueries = A.Fake<ICustomerQueries>();

        var customersDataModel = new List<CustomerDataModel>
        {
            new()
            {
                Id = 1,
                Name = "name 1"
            },
            new()
            {
                Id = 2,
                Name = "name 2"
            }
        };

        A.CallTo(() => customerQueries.GetList(filter)).Returns(customersDataModel);
        
        var controller = new CustomerController(
            A.Fake<ILogger<CustomerController>>(),
            A.Fake<ICustomerCommands>(),
            customerQueries);
        
        // Act
        var actionResult = await controller.Get(filter);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

        var actualModel = Assert.IsType<List<CustomerModel>>(okResult.Value);

        for (var i = 0; i < customersDataModel.Count; i++)
        {
            Assert.Equal(customersDataModel[i].Id, actualModel[i].Id);
            Assert.Equal(customersDataModel[i].Name, actualModel[i].Name);
        }
        
        A.CallTo(() => customerQueries.GetList(filter)).MustHaveHappenedOnceExactly();
    }
}