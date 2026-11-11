using KASHOP.DAL.DTO.Request;

namespace KASHOP.BLL.Settings
{
    public interface IAuthService
    {
        Task<(bool Success, List<string> Errors)> RegisterAsync(
            RegisterRequest request
            );
    }
}
