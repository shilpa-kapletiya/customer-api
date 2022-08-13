using Customer.Api.Infrastructure.Data;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        const string sql = 
            @"select id, name from customer 
              where id = @id";

        using var connection = await this._connectionProvider.GetConnection();

        return await connection.QuerySingleOrDefaultAsync<CustomerDataModel>
                                                         (sql, new { id });
    }

    public async Task<List<CustomerDataModel>> GetList(string filter)
    {
        var sql = @"select id, name from customer";

        if (!string.IsNullOrEmpty(filter))
        {
            sql += $" where name ilike '%{filter}%'";
        }
        
        using var connection = await this._connectionProvider.GetConnection();

        var results = await connection
                                            .QueryAsync<CustomerDataModel>(sql);
        return results.ToList();
    }
}