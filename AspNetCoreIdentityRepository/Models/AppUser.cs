using AspNetCoreIdentityApp.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityRepository.Models
{
    public class AppUser :IdentityUser
    {
        public string? City { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Birthday { get; set; }
        public string? Picture { get; set; }
        public Gender? Gender { get; set; }

    }

}
