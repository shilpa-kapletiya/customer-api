using Customer.Api.Features.Customer.Data;
using Customer.Api.Features.Customer.Validators;
using Customer.Api.Infrastructure;
using Customer.Api.Models;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customer.Api.Controllers;

[ApiController]
[Route(ApiRoute.Customers)]
public class CustomerController : ControllerBase
{
    private readonly ILogger<CustomerController> _logger;
    private readonly ICustomerCommands _customerCommands;
    private readonly ICustomerQueries _customerQueries;

    public CustomerController(
        ILogger<CustomerController> logger, 
        ICustomerCommands customerCommands,
        ICustomerQueries customerQueries)
    {
        _logger = logger;
        _customerCommands = customerCommands;
        _customerQueries = customerQueries;
    }
    
    /// <summary>
    /// Get list of customers
    /// </summary>
    /// <param name="filter">optional filter by name</param>
    /// <returns>Return list of customers</returns>
    [HttpGet]
    public async Task<ActionResult<List<CustomerModel>>> Get(string filter = null)
    {
        var dataModels = await _customerQueries.GetList(filter);

        var models = dataModels
            .Select(d => new CustomerModel { Id = d.Id,Name = d.Name })
            .ToList();
        
        return this.Ok(models);
    }

    /// <summary>
    /// Get a customer
    /// </summary>
    /// <param name = "id">Customer Identifier</param>
    /// <returns>Return list of customers</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerModel>> Get(int id)
    {
        var dataModel = await _customerQueries.Get(id);

        if (dataModel == null)
        {
            return NotFound();
        }

        return this.Ok(new CustomerModel{ Id = dataModel.Id, Name = dataModel.Name});
    }
    
    /// <summary>
    /// Create a new customer
    /// </summary>
    /// <param name ="model">Customer data</param>
    /// <returns>Customer identifier</returns>
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CustomerCreateModel model)
    {
        var validator = new CustomerCreateModelValidator();
        var validationResult = await validator.ValidateAsync(model);
        validationResult.AddToModelState(this.ModelState, null);

        if (!this.ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var id = await _customerCommands.Create(new CustomerCreateDataModel 
            { Name = model.Name });

        return this.CreatedAtAction(nameof(this.Get), new{ id },  
            new CustomerModel{ Id = id, Name = model.Name });
    }

    /// <summary>
    /// Delete customer
    /// </summary>
    /// <param name = "id">Customer Identifier</param>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
         await _customerCommands.Delete(id);

         return NoContent();
    }

    [HttpPut]
    public async Task<ActionResult> Put(int id, [FromBody] CustomerUpdateModel model)
    {
        var validator = new CustomerUpdateModelValidator();
        var validationResult = await validator.ValidateAsync(model);
        validationResult.AddToModelState(this.ModelState, null);

        if (!this.ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        await _customerCommands.Update(id, new CustomerUpdateDataModel
        {
            Name = model.Name
        });
        
        return NoContent();
    }
    
    [HttpPatch]
    public async Task<ActionResult> Patch(int id, 
        [FromBody] JsonPatchDocument<CustomerUpdateModel> patchDocument)
    {
        var dataModel = await _customerQueries.Get(id);

        if (dataModel == null)
        {
            return NotFound();
        }

        var updateModel = new CustomerUpdateModel { Name = dataModel.Name };
        
        patchDocument.ApplyTo(updateModel);
        
        var validator = new CustomerUpdateModelValidator();
        var validationResult = await validator.ValidateAsync(updateModel);
        validationResult.AddToModelState(this.ModelState, null);

        if (!this.ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        await _customerCommands.Update(id, new CustomerUpdateDataModel
        {
            Name = updateModel.Name
        });
        
        return NoContent();
    }
}