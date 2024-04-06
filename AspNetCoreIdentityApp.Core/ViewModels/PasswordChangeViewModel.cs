using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AspNetCoreIdentityApp.Core.ViewModels
{
    public class PasswordChangeViewModel
    {
        [DataType(DataType.Password)]
        [DisplayName("Eski Şifre")]
        [Required(ErrorMessage = "Eski Şifre alanı boş bırakılamaz.")]
        [MinLength(6,ErrorMessage ="Şifreniz en az 6 karakter olabilir.")]
        public string? PasswordOld { get; set; }


        [DataType(DataType.Password)]
        [DisplayName("Yeni Şifre")]
        [Required(ErrorMessage = " Yeni Şifre alanı boş bırakılamaz.")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir.")]
        public string? PasswordNew { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Yeni Şifre Tekrar")]
        [Compare(nameof(PasswordNew), ErrorMessage = "Şifreler uyuşmuyor.")]
        [Required(ErrorMessage = " Yeni Şifre tekrar alanı boş bırakılamaz.")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir.")]
        public string? PasswordNewConfirm { get; set; }

    }
}
