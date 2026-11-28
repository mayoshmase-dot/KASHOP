using KASHOP.BLL.Common;
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
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<Category> _categoryRepo;
        public ProductService(IFileService fileService,IGenericRepository<Product> productRepo, IGenericRepository<Category> categoryRepo)
        {
            _fileService = fileService;
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }
        public async Task<List<ProductResponses>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var Products = await _productRepo.GetAsync(
                includes: [p => p.Translations, p => p.Category.Translations],
                cancellationToken: cancellationToken
                );
            return Products.Adapt<List<ProductResponses>>();

        }
        public async Task<Result<ProductResponses>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var product = await _productRepo.GetOneAsync(
                filter: p => p.Id == id,
                includes: [p => p.Translations, p => p.Category.Translations],
                cancellationToken: cancellationToken
                );
            if (product is null)
            return Result<ProductResponses>.NotFound(["Product Not Found"]);
            return Result<ProductResponses>.Success(product.Adapt<ProductResponses?>());

        }
        public async Task <Result<bool>> CreateAsync(ProductRequest request, CancellationToken cancellationToken = default)
        {
            if (!AreTranslationsValid(request.Translations))
                return Result<bool>.Validation(["Translations must include 'en' and 'ar' exactly once"]);

            if (!await CategoryExistsAsync(request.CategoryId , cancellationToken))
             return Result<bool>.Validation([$"Category with : {request.CategoryId} does not exist"]);
            if(request.MainImage is null)
                return Result<bool>.Validation(["Main image is requierd"]);

            var ImageName = await _fileService.UploadAsync(request.MainImage, cancellationToken);
            if (ImageName is null)
                return Result<bool>.Validation([$"Image must be jpg , jpeg , png or webp and less 2MB"]);

            var product = request.Adapt<Product>();
            product.MainImage = ImageName;
            try
            {
                await _productRepo.CreateAsync(product, cancellationToken);
            }
            catch
            {
                _fileService.Delete(ImageName);
                throw;
            }
            return Result<bool>.Success(true);
        }
        public async Task<Result<bool>> UpdateAsync(int Id, ProductRequest request, CancellationToken cancellationToken = default)
        {
            if (!AreTranslationsValid(request.Translations))
                return Result<bool>.Validation(["Translations must include 'en' and 'ar' exactly once"]);

            if (!await CategoryExistsAsync(request.CategoryId, cancellationToken))
                return Result<bool>.Validation([$"Category with : {request.CategoryId} does not exist"]);

            var product = await _productRepo.GetOneAsync(
              filter: p => p.Id == Id,
              includes: [p => p.Translations],
              tracked: true,
              cancellationToken: cancellationToken
              );
            if (product is null)
                return Result<bool>.NotFound(["Product Not Found"]);
            string? oldImage = null;
            if (request.MainImage is not null) {

                var newImage = await _fileService.UploadAsync(request.MainImage, cancellationToken);

                if (newImage is null)
                    return Result<bool>.Validation([$"Image must be jpg , jpeg , png or webp and less 2MB"]);
                oldImage = product.MainImage;
                product.MainImage = newImage;
            }
            product.Price = request.Price;
            product.Discount = request.Discount;
            product.Quantity = request.Quantity;
            product.CategoryId = request.CategoryId;

            var names = request.Translations.Select(x => x.Name.ToLower()).ToList();
            var dublicate = await _productRepo.GetOneAsync(
               c => c.Id != Id && c.Translations.Any(x => names.Contains(x.Name.ToLower())),
                cancellationToken: cancellationToken
                );
            if (dublicate is not null)
                return Result<bool>.Conflict(["Category with this name already exists"]);
            foreach (var translation in request.Translations)
            {
                var existingTranslation = product.Translations.FirstOrDefault(t => t.Language == translation.Language);
                existingTranslation.Name = translation.Name;
            }
            try
            {
                await _productRepo.UpdateAsync(product, cancellationToken);
            }
            catch
            {
                _fileService.Delete(product.MainImage);
                throw;
            }
            if(oldImage is not null)
            {
                _fileService.Delete(oldImage);

            }
            return Result<bool>.Success(true);
        }
        public async Task<Result<bool>> RemoveAsync(int id, CancellationToken cancellationToken = default)
        {
            var product = await _productRepo.GetOneAsync(
                filter: c => c.Id == id,
                includes: [c => c.Translations],
                cancellationToken: cancellationToken);

            if (product is null)
                return Result<bool>.NotFound(["Category not found"]);

            var imageName = product.MainImage;

            await _productRepo.RemoveAsync(product, cancellationToken);
            _fileService.Delete(imageName);
            return Result<bool>.Success(true);
        }

        private async Task<bool> CategoryExistsAsync(int categoryId , CancellationToken cancellationToken = default)
        {
            var category = await _categoryRepo.GetOneAsync(
                 filter: c => c.Id == categoryId,
                cancellationToken: cancellationToken
                );
            return category is not null; 

        }
        private static bool AreTranslationsValid(List<ProductTranslationRequest> translations)
        {
            var Languages = translations.Select(x => x.Language.ToLower()).ToList();
            if (Languages.Distinct().Count() != Languages.Count) return false;
            return Languages.Count == SupportedLanguages.Languages.Length && SupportedLanguages.Languages.All(x => Languages.Contains(x));
        }

    }
}
