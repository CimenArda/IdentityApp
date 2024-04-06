using AspNetCoreIdentityApp.Core.OptionsModels;
using Microsoft.Extensions.Options;

namespace AspNetCoreIdentity.Web.Services
{
    public interface IEmailService
    {
      
        Task SendResetPasswordEmail(string resetEmailLink, string ToEmail);
    }
}
