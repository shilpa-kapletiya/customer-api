using Customer.Api.Infrastructure.Data;
using Dapper;
using System.Data;

namespace Customer.Api.Features.Customer.Data;

public interface ICustomerCommands
{
    Task<int> Create(CustomerCreateDataModel model);
}

public class CustomerCommands : ICustomerCommands
{
    private readonly IDbConnectionProvider _dbConnectionProvider;

    public CustomerCommands(IDbConnectionProvider dbConnectionProvider)
    {
        _dbConnectionProvider = dbConnectionProvider;
    }
    
    public async Task<int> Create(CustomerCreateDataModel model)
    {
        const string sql = @"insert into customer(name) 
                             values(@name) 
                             returning id;";

        using var connection = await this._dbConnectionProvider.GetConnection();

        return await connection.QuerySingleAsync<int>(sql, model);
    }
    
}
