using Customer.Api.Infrastructure.Data;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Customer.Api.Features.Customer.Data;

public interface ICustomerQueries
{
    Task<CustomerDataModel> Get(int id);

    Task<List<CustomerDataModel>> GetList(string filter);
}