using ClientManager.Model.Common;
using ClientManager.Model.Result;
using System.Threading.Tasks;

namespace ClientManager.Service.Interfaces
{
    public interface IUserService
    {
        Task<Result<bool>> Validate(User user);
    }
}