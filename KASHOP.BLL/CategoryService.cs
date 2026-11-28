using KASHOP.BLL.Common;
using KASHOP.BLL.Settings;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Responses;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repositories;
using Mapster;

namespace KASHOP.BLL
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _repo;
        public CategoryService(IGenericRepository<Category> repo)
        {
            _repo = repo;
        }
        public async Task<List<CategoryResponses>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var categories = await _repo.GetAsync(
                includes: [c => c.Translations],
                cancellationToken: cancellationToken);

            return categories.Adapt<List<CategoryResponses>>();
        }

        public async Task<Result<CategoryResponses>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var category = await _repo.GetOneAsync(
                filter: c => c.Id == id,
                includes: [c => c.Translations],
                cancellationToken: cancellationToken
                );
            if (category is null)
                return Result<CategoryResponses>.NotFound(["Category Not Found"]);
            return Result<CategoryResponses>.Success(category.Adapt<CategoryResponses?>());

        }
        public async Task<Result<bool>> CreateAsync(CategoryRequest request , CancellationToken cancellationToken = default)
        {
            if(!AreTranslationsValid(request.Translations)) 
                return Result<bool>.Validation(["Translations must include 'en' and 'ar' exactly once"]);
            var names = request.Translations.Select(x => x.Name.ToLower()).ToList();
            var exisiting = await _repo.GetOneAsync(
                filter: c => c.Translations.Any(x => names.Contains(x.Name.ToLower())),
                cancellationToken: cancellationToken
                );
            if (exisiting is not null)
                return Result<bool>.Conflict(["Category with this name already exists"]);

            var category = request.Adapt<Category>();
            await _repo.CreateAsync(category , cancellationToken );
            return Result<bool>.Success(true);
        }
        public async Task<Result<bool>> UpdateAsync(int Id, CategoryRequest request, CancellationToken cancellationToken = default)
        {
            if (!AreTranslationsValid(request.Translations))
                return Result<bool>.Validation(["Translations must include 'en' and 'ar' exactly once"]);
            var category = await _repo.GetOneAsync(
              filter: c => c.Id == Id,
              includes: [c => c.Translations],
              tracked: true,
              cancellationToken: cancellationToken
              );
            if (category is null)
                return Result<bool>.NotFound(["Category Not Found"]);
            var names = request.Translations.Select(x => x.Name.ToLower()).ToList();
            var dublicate = await _repo.GetOneAsync(
               c => c.Id != Id && c.Translations.Any(x => names.Contains(x.Name.ToLower())),
                cancellationToken: cancellationToken
                );
            if (dublicate is not null)
                return Result<bool>.Conflict(["Category with this name already exists"]);
            category.Status = request.Status;
            foreach (var translation in request.Translations)
            {
               var existingTranslation = category.Translations.FirstOrDefault(t=>t.Language == translation.Language);
            existingTranslation.Name = translation.Name;
            }
            await _repo.UpdateAsync(category, cancellationToken);
            return Result<bool>.Success(true);
        }
        public async Task<Result<bool>> RemoveAsync(int id, CancellationToken cancellationToken = default)
        {
            var category = await _repo.GetOneAsync(
                filter: c => c.Id == id,
                includes: [c => c.Translations],
                cancellationToken: cancellationToken);

            if (category is null)
                return Result<bool>.NotFound(["Category not found"]);

            var hasProducts = await _repo.GetOneAsync(
                filter: c => c.Id == id && c.Products.Any(),
                cancellationToken: cancellationToken);

            if (hasProducts is not null)
                return Result<bool>.Conflict(["Can't delete a category that has products"]);

            await _repo.RemoveAsync(category, cancellationToken);
            return Result<bool>.Success(true);
        }
        
        private static bool AreTranslationsValid(List<CategoryTranslationRequest> translations)
        {
         var Languages = translations.Select(x => x.Language.ToLower()).ToList();
            if(Languages.Distinct().Count() != Languages.Count) return false;
            return Languages.Count ==  SupportedLanguages.Languages.Length && SupportedLanguages.Languages.All(x =>Languages.Contains(x));
        }
    }
}
