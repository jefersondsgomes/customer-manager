using CustomerManager.Model.Result;
using CustomerManager.Model.Transient;
using System.Threading.Tasks;

namespace CustomerManager.Service.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result<AuthenticateResponse>> AuthenticateAsync(AuthenticateRequest authenticateRequest);
    }
}
