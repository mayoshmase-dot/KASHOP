using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Responses;

namespace KASHOP.BLL.Settings
{
    public interface IAuthService
    {
        Task<(bool Success, List<string> Errors)> RegisterAsync(
            RegisterRequest request);
        Task<(bool Success, List<string> Errors, LoginResponses response)> LoginAsync(LoginRequest request);

    }
}
