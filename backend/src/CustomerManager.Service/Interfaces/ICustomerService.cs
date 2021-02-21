using CustomerManager.Model.Common;
using CustomerManager.Model.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerManager.Service.Interfaces
{
    public interface ICustomerService
    {
        Task<Result<Customer>> GetAsync(string id);
        Task<Result<ICollection<Customer>>> GetAllAsync();
        Task<Result<Customer>> CreateAsync(Customer customer);
        Task<Result<Customer>> UpdateAsync(string id, Customer customer);
        Task<Result<bool>> DeleteAsync(string id);
    }
}