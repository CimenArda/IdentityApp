using AspNetCoreIdentityApp.Core.OptionsModels;
using Microsoft.Extensions.Options;

namespace AspNetCoreIdentityService.Services
{
    public interface IEmailService
    {
      
        Task SendResetPasswordEmail(string resetEmailLink, string ToEmail);
    }
}
