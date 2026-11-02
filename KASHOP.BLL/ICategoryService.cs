using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Responses;
using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP.BLL
{
    public interface ICategoryService
    {
        Task<List<CategoryResponses>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<CategoryResponses?> GetByIdAsync(int id , CancellationToken cancellationToken = default);
        Task<bool> CreateAsync(CategoryRequest request, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(int Id ,CategoryResponses request , CancellationToken cancellationToken = default);
        Task<bool> RemoveAsync(int Id , CancellationToken cancellationToken = default);

    }
}
