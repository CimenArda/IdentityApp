using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Web.CustomValidatons
{
    public class PasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string? password)
        {

            var errors = new List<IdentityError>();


            // ! işaretinin run-time da bir önemi yok.Bir ifadenin null olmadığını garanti etmek için kullanılır.
            if (password!.ToLower().Contains(user.UserName!.ToLower()))
            {
                errors.Add(new() { Code = "PasswordContaionUserName", Description = "Şifre alanı kullanıcı adı içeremez." });
            }
            if (password.ToLower().StartsWith("123"))
            {
                errors.Add(new() { Code = "PasswordContaion123", Description = "Şifre alanı ardışık sayı içeremez." });
            }

            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
            
            return Task.FromResult(IdentityResult.Success);
   

        }

        



    }
}
