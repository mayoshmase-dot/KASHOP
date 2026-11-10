using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP.BLL.Settings
{
    public interface IFileService
    {
        Task<string?> UploadAsync(IFormFile file , CancellationToken cancellationToken = default);
        void Delete(string FileName);
    }
}
