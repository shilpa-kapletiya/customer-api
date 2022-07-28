using Customer.Api.Infrastructure.Data;
using Dapper;

namespace Customer.Api.Features.Customer.Data;

public interface ICustomerQueries
{
    Task<CustomerDataModel> Get(int id);
    
}