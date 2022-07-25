using Customer.Api.Infrastructure;
using Customer.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Customer.Api.Controllers;

[ApiController]
[Route(ApiRoute.Customers)]
public class CustomerController : ControllerBase
{
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(ILogger<CustomerController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Get a customer
    /// </summary>
    /// <param name="id">Customer Identifier</param>
    /// <returns>Return list of customers</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerModel>> Get(int id)
    {
        await Task.Delay(10);

        return this.Ok(new CustomerModel{ Id = id, Name = $"dummy {id}"});
    }
    
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CustomerCreateModel model)
    {
        await Task.Delay(10);

        // todo:
        // 1. add db access layer
        // 2. add validation
        // 3. add swagger doc comments
        // 4. add automapper
        // 5. add unit tests
        
        var id = 1;

        return this.CreatedAtAction(nameof(this.Get), new{ id },  new CustomerModel{ Id = id, Name = model.Name });
    }

    
    // [HttpGet(Name = "GetWeatherForecast")]
    // public IEnumerable<WeatherForecast> Get()
    // {
    //     return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //         {
    //             Date = DateTime.Now.AddDays(index),
    //             TemperatureC = Random.Shared.Next(-20, 55),
    //             Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //         })
    //         .ToArray();
    // }
}