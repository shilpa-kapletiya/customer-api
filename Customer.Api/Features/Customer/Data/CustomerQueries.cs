using Customer.Api.Infrastructure.Data;
using Dapper;

namespace Customer.Api.Features.Customer.Data;

public class CustomerQueries : ICustomerQueries
{
    private readonly IDbConnectionProvider _connectionProvider;

    public CustomerQueries(IDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }
    
    public async Task<CustomerDataModel> Get(int id)
    {
        const string sql = @"select * from customer 
                             where id = @id;";

        using var connection = await this._connectionProvider.GetConnection();

        return await connection.QuerySingleOrDefaultAsync<CustomerDataModel>(sql, new { id });
    }
}