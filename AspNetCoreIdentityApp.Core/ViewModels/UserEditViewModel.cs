using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using AspNetCoreIdentityApp.Core.Models;
using Microsoft.AspNetCore.Http;

namespace AspNetCoreIdentityApp.Core.ViewModels
{
    public class UserEditViewModel
    {


        [DisplayName("Kullanıcı Adı")]
        [Required(ErrorMessage = "Kullanıcı Adı boş bırakılamaz.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Email giriş formatınız hatalı.")]
        public string Email { get; set; }

        [DisplayName("Telefon")]
        [Required(ErrorMessage = "Telefon alanı boş bırakılamaz.")]
        public string Phone { get; set; }

        [DisplayName("Doğum Tarihi")]
        [Required(ErrorMessage = "Doğum Tarih alanı boş bırakılamaz.")]
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }

        [DisplayName("Şehir")]
        public string City { get; set; }

        
        [DisplayName("Resim")]
        public IFormFile? Picture { get; set; }

        [DisplayName("Cinsiyet")]
        public Gender? Gender { get; set; }


    }
}
