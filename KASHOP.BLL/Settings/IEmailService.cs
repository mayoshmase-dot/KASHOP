using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP.BLL.Settings
{
    public interface IEmailService
    {
        Task SendEmailAsync (string to , string subject , string htmlBody);

    }
}
