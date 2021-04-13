using CustomerManager.Model.Common;
using CustomerManager.Model.Result;
using System.Threading.Tasks;

namespace CustomerManager.Service.Interfaces
{
    public interface IUserService
    {
        Task<Result<bool>> AuthenticateAsync(User user);
        Task<Result<User>> CreateAsync(User user);
    }
}