using KASHOP.BLL.Common;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Responses;

namespace KASHOP.BLL
{
    public interface IProductService
    {
        Task<List<ProductResponses>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<ProductResponses>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<bool>> CreateAsync(ProductRequest request, CancellationToken cancellationToken = default);
        Task<Result<bool>> UpdateAsync(int Id, ProductRequest request, CancellationToken cancellationToken = default);
        Task<Result<bool>> RemoveAsync(int Id, CancellationToken cancellationToken = default);

    }
}
