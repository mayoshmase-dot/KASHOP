using KASHOP.BLL.Settings;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Responses;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repositories;
using Mapster;


namespace KASHOP.BLL
{
    public class ProductService : IProductService
    {
        private readonly IFileService _fileService;
        private readonly IGenericRepository <Product> _productRepo;
        public ProductService(IFileService fileService,IGenericRepository<Product> productRepo)
        {
            _fileService = fileService;
            _productRepo = productRepo;
        }

        public async Task<bool> CreateAsync(ProductRequest request, CancellationToken cancellationToken = default)
        {
            var ImageName = await _fileService.UploadAsync(request.MainImage, cancellationToken);
            if(ImageName is  null) return false;
            var product = request.Adapt<Product>();
            product.MainImage = ImageName;
            try
            {
                await _productRepo.CreateAsync(product, cancellationToken);
            }
            catch{
                _fileService.Delete(ImageName);
                throw;
            }
              return true;
        }

        public async Task<List<ProductResponses>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var Products = await _productRepo.GetAsync(
                includes: [p => p.Translations , p=>p.Category.Translations],
                cancellationToken: cancellationToken
                );
            return Products.Adapt<List<ProductResponses>>();

        }
    }
}
