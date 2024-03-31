using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.Web.ViewModels
{
    public class ForgetPasswordViewModel
    {

        [Required(ErrorMessage = "Email boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Email giriş formatınız hatalı.")]
        public string? Email { get; set; }




    }
}
