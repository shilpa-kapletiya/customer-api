using System;

namespace Customer.Api.Models;

public class CustomerUpdateModel : ICustomerCommonModel
{
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
}