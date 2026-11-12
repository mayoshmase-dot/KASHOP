using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP.BLL.Settings
{
    public interface ITokenService
    {
        Task<string> GenerateJwt(ApplictionUser user);

    }
}
