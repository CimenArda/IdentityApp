using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Web.Localizations
{
    public class LocalizationIdentityErrorDescriber :IdentityErrorDescriber
    {



        public override IdentityError DuplicateUserName(string userName)
        {

            return new() { Code = "DuplicateUserName", Description = $"{userName} daha önce başka bir kullanıcı tarafından alınmıştır." };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new() { Code = "DuplicateEmail", Description = "Bu email daha önce başka bir kullanıcı tarafından alınmıştır." };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new() { Code = "PasswordTooShort",Description="Şifreniz en az 8 karakter olmalıdır" };
        }



    }



}
