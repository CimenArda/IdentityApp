using AspNetCoreIdentity.Web.Areas.Admin.Models;
using AspNetCoreIdentityRepository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentity.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public HomeController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UserList()
        {
            var userlList = await _userManager.Users.ToListAsync();

            var userViewModel = userlList.Select(x => new UserViewModel()
            {
                Email = x.Email,
                UserId = x.Id,
                UserName = x.UserName
            }).ToList();
            return View(userViewModel);
        }
    }
}
