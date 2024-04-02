using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.Web.Areas.Admin.Models
{
    public class RoleCreateViewModel
    {
        [Display(Name = "Rol İsim Alanı")]
        [Required(ErrorMessage ="Bu Alan boş geçilemez.")]
        public string Name { get; set; }
    }
}
