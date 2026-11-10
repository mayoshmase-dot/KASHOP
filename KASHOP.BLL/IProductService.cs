using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Responses;

namespace KASHOP.BLL
{
    public interface IProductService
    {
        Task<bool> CreateAsync(ProductRequest request, CancellationToken cancellationToken = default);
        Task<List< ProductResponses>>GetAllAsync (CancellationToken cancellationToken = default);

    }
}
