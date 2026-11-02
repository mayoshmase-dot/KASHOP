using KASHOP.BLL.Settings;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Responses;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repositories;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP.BLL
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _repo;
        public CategoryService(IGenericRepository<Category> repo)
        {
            _repo = repo;
        }
        public async Task<bool> CreateAsync(CategoryRequest request , CancellationToken cancellationToken = default)
        {
            if(!AreTranslationsValid(request.Translations)) return false;
            var names = request.Translations.Select(x => x.Name.ToLower()).ToList();
            var exisiting = await _repo.GetOneAsync(
                filter: c => c.Translations.Any(x => names.Contains(x.Name.ToLower())),
                cancellationToken: cancellationToken
                );
            if (exisiting is not null) return false;
           
            var category = request.Adapt<Category>();
            await _repo.CreateAsync(category , cancellationToken );
            return true;

        }

        public async Task<List<CategoryResponses>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var categories = await _repo.GetAsync(
                includes: [c=>c.Translations],
                cancellationToken:cancellationToken);

            return categories.Adapt<List<CategoryResponses>>();
        }

        public async Task<CategoryResponses?> GetByIdAsync(int id , CancellationToken cancellationToken = default)
        {
            var categories = await _repo.GetOneAsync(
                filter: c => c.Id == id,
                includes: [c => c.Translations],
                cancellationToken: cancellationToken
                );

            return categories.Adapt<CategoryResponses?>();
        }

        public async Task<bool> RemoveAsync(int Id , CancellationToken cancellationToken = default)
        {
            var category = await _repo.GetOneAsync(
                filter: c => c.Id == Id,
                includes: [c => c.Translations],
                cancellationToken: cancellationToken
                );
            if(category is   null) return false;
            await _repo.RemoveAsync(category, cancellationToken);
            return true;
        }

        public Task<bool> UpdateAsync(int Id, CategoryResponses request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        private static bool AreTranslationsValid(List<CategoryTranslationRequest> translations)
        {
         var Languages = translations.Select(x => x.Language.ToLower()).ToList();
            if(Languages.Distinct().Count() != Languages.Count) return false;
            return Languages.Count ==  SupportedLanguages.Languages.Length && SupportedLanguages.Languages.All(x =>Languages.Contains(x));
        }
    }
}
