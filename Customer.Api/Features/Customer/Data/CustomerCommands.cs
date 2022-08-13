using Customer.Api.Infrastructure.Data;
using Dapper;
using System.Threading.Tasks;

namespace Customer.Api.Features.Customer.Data;

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
                             returning id";

        using var connection = await this._dbConnectionProvider.GetConnection();

        return await connection.QuerySingleAsync<int>(sql, model);
    }

    public async Task Update(int id, CustomerUpdateDataModel model)
    {
        const string sql = @"update customer
                             set name = @name
                             where id = @id";
        
        using var connection = await this._dbConnectionProvider.GetConnection();

        await connection.ExecuteAsync(sql, new { id , name = model.Name });
    }

    public async Task Delete(int id)
    {
        const string sql = @"delete from customer 
                             where id = @id";
        
        using var connection = await this._dbConnectionProvider.GetConnection();

        await connection.ExecuteAsync(sql, new { id });
    }
}
