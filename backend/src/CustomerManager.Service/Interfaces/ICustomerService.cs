using CustomerManager.Models.Entities;
using CustomerManager.Models.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerManager.Services.Interfaces

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