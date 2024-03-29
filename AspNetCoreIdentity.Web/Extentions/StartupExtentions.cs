using AspNetCoreIdentity.Web.CustomValidatons;
using AspNetCoreIdentity.Web.Localizations;
using AspNetCoreIdentity.Web.Models;

namespace AspNetCoreIdentity.Web.Extentions
{
    public static class StartupExtentions
    {
        public static void AddIdentityWithExtentions(this IServiceCollection services)
        {

           services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnoprstuvyzxwq1234567890._ABCDEFGHTREQWUYPLKMMNZXCVL";

                opt.Password.RequiredLength = 8;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireDigit = false;


                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                opt.Lockout.MaxFailedAccessAttempts = 3;




            }).AddPasswordValidator<PasswordValidator>().AddUserValidator<UserValidator>().AddErrorDescriber<LocalizationIdentityErrorDescriber>().AddEntityFrameworkStores<AppDbContext>();


        }


    }
}
