using Customer.Api.Controllers;
using Customer.Api.Features.Customer.Data;
using Customer.Api.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Customer.Api.Unit.Tests;

[Trait("Category","Customer")]
public class PatchCustomerTests
{
    [Fact]
    public async Task Should_Patch_Customer()
    {
        // Arrange
        var customerCommands = A.Fake<ICustomerCommands>();
        var customerQueries = A.Fake<ICustomerQueries>();
        
        const int id = 1;
        
        var customerDataModel = new CustomerDataModel
        {
            Id = id,
            Name = "a test customer"
        };

        A.CallTo(() => customerQueries.Get(id)).Returns(customerDataModel);
        
        const string updatedName = "updated customer name";

        var patchDocument = new JsonPatchDocument<CustomerUpdateModel>();
        patchDocument.Replace(c => c.Name, updatedName);
        
        var controller = new CustomerController(
            A.Fake<ILogger<CustomerController>>(),
            customerCommands,
            customerQueries);
        
        // Act
        var actionResult = await controller.Patch(id, patchDocument);
        
        // Assert
        Assert.IsType<NoContentResult>(actionResult);

        A.CallTo(() =>
            customerCommands.Update(id, A<CustomerUpdateDataModel>.
                That.Matches(c => c.Name == updatedName)))
                .MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task Should_Return_Not_Found_For_Unknown_Customer_Id()
    {
        // Arrange
        const int unknownCustomerId = 99999;
        
        var customerCommands = A.Fake<ICustomerCommands>();
        var customerQueries = A.Fake<ICustomerQueries>();
        
        const string updatedName = "updated customer name";

        var patchDocument = new JsonPatchDocument<CustomerUpdateModel>();
        patchDocument.Replace(c => c.Name, updatedName);

        A.CallTo(() => customerQueries.Get(unknownCustomerId)).Returns((CustomerDataModel)null);
        
        var controller = new CustomerController(
            A.Fake<ILogger<CustomerController>>(),
            customerCommands,
            customerQueries);
        
        // Act
        var actionResult = await controller.Patch(unknownCustomerId, patchDocument);
        
        // Assert
        Assert.IsType<NotFoundResult>(actionResult);
        
        A.CallTo(() => customerQueries.Get(unknownCustomerId)).MustHaveHappenedOnceExactly();
        
        A.CallTo(() =>
                customerCommands.Update(unknownCustomerId, A<CustomerUpdateDataModel>.Ignored))
            .MustNotHaveHappened();
    }
}