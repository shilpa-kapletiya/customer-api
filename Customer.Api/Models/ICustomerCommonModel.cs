using System;

namespace Customer.Api.Models;

public interface ICustomerCommonModel
{
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
}