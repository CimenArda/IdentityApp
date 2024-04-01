using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AspNetCoreIdentity.Web.ViewModels
{
    public class ResetPasswordViewModel
    {
        [DataType(DataType.Password)]
        [DisplayName("Yeni Şifre")]
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [DisplayName(" Yeni Şifre Tekrar")]
        [Compare(nameof(Password), ErrorMessage = "Şifreler uyuşmuyor.")]
        [Required(ErrorMessage = "Şifre tekrar alanı boş bırakılamaz.")]
        public string PasswordConfirm { get; set; }
    }
}
