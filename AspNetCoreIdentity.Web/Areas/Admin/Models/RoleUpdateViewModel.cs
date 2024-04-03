using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.Web.Areas.Admin.Models
{
    public class RoleUpdateViewModel
    {
        public string? Id { get; set; }

        [Display(Name = "Rol İsim Alanı")]
        [Required(ErrorMessage = "Bu Alan boş geçilemez.")]
        public string? Name { get; set; }
    }
}
