using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP.DAL.DTO.Request
{
    public class CategoryRequest
    {
        public bool Status { get; set; }
        public List<CategoryTranslationRequest> Translations { get; set; } = new();
    }
}
