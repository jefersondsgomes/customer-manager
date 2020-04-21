using System.Threading.Tasks;
using ClientManager.Model.Result;
using ClientManager.Model.User;

namespace ClientManager.Service.Interfaces
{
    public interface IUserService
    {
        Task<Result<bool>> ValidadeUser(User user);         
    }
}