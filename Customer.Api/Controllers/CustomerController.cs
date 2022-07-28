using Customer.Api.Features.Customer.Data;
using Customer.Api.Infrastructure;
using Customer.Api.Models;
using Microsoft.AspNetCore.Mvc;

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
    /// Get a customer
    /// </summary>
    /// <param name="id">Customer Identifier</param>
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
    /// <param name="model">Customer data</param>
    /// <returns>Customer identifier</returns>
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CustomerCreateModel model)
    {
        // todo:
        // 2. add validation
        // 3. add swagger doc comments
        // 4. add automapper
        // 5. add unit tests

        var id = await _customerCommands.Create(new CustomerCreateDataModel { Name = model.Name });

        return this.CreatedAtAction(nameof(this.Get), new{ id },  new CustomerModel{ Id = id, Name = model.Name });
    }
}