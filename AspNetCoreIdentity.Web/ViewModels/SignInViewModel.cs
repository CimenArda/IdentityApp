﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.Web.ViewModels
{
    public class SignInViewModel
    {
        [Required(ErrorMessage = "Email boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Email giriş formatınız hatalı.")]
        public string? Email { get; set; }
      
        [DisplayName("Şifre")]
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }


    }
}
