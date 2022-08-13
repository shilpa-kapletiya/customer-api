using System.Threading.Tasks;

namespace Customer.Api.Features.Customer.Data;

public interface ICustomerCommands
{
    Task<int> Create(CustomerCreateDataModel model);
    
    Task Update(int id, CustomerUpdateDataModel model);
    
    Task Delete(int id);
}