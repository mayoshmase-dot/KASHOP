using KASHOP.BLL.Common;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Responses;

namespace KASHOP.BLL
{
    public interface ICategoryService
    {
        Task<List<CategoryResponses>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<CategoryResponses?> GetByIdAsync(int id , CancellationToken cancellationToken = default);
        Task<bool> CreateAsync(CategoryRequest request, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(int Id ,CategoryResponses request , CancellationToken cancellationToken = default);
        Task <Result<bool>> RemoveAsync(int Id , CancellationToken cancellationToken = default);

    }
}
