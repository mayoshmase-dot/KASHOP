using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP.DAL.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null;
        public bool Status { get; set; }

    }
}
