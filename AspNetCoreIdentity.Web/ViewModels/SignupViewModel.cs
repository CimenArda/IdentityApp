using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.Web.ViewModels
{
    public class SignupViewModel
    {
        [DisplayName("Kullanıcı Adı")]
        [Required(ErrorMessage ="Kullanıcı Adı boş bırakılamaz.")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Email boş bırakılamaz.")]
        [EmailAddress(ErrorMessage ="Email giriş formatınız hatalı.")]
        public string Email { get; set; }
        
        [DisplayName("Telefon")]
        [Required(ErrorMessage = "Telefon alanı boş bırakılamaz.")]
        public string Phone { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Şifre")]
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        public string Password { get; set; }
       
        [DataType(DataType.Password)]
        [DisplayName("Şifre Tekrar")]
        [Compare(nameof(Password),ErrorMessage = "Şifreler uyuşmuyor.")]
        [Required(ErrorMessage = "Şifre tekrar alanı boş bırakılamaz.")]
        public string PasswordConfirm { get; set; }


    }
}
