using CustomerManager.Models.Entities;
using CustomerManager.Models.Results;
using System.Threading.Tasks;

namespace CustomerManager.Services.Interfaces
{
    public interface IUserService
    {
        Task<Result<bool>> AuthenticateAsync(User user);
        Task<Result<User>> CreateAsync(User user);
        Task<Result<User>> GetAsync(string id);
    }
}