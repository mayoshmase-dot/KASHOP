using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KASHOP.DAL.DTO.Responses
{
    public class LoginResponses
    {
        public string Token { get; set; }
        public DateTime ExipsetAt { get; set; }
       
    }
}
