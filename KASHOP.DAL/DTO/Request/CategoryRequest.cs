using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP.DAL.DTO.Request
{
    public class CategoryRequest
    {
        public string? Status { get; set; }
        public List<CategoryTranslationRequest> Translations { get; set; } = new();
    }
}
