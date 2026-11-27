using KASHOP.BLL.Common;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Responses;

namespace KASHOP.BLL.Settings
{
    public interface IAuthService
    {
        Task<Result<bool>> RegisterAsync(RegisterRequest request);
        Task<Result < LoginResponses >> LoginAsync(LoginRequest request);
        Task<Result<bool>> ConfirmEmailAsync(string userId, string token);


    }
}
