using AspNetCoreIdentity.Web.Areas.Admin.Models;
using AspNetCoreIdentity.Web.Extentions;
using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public RoleController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.Select(x => new RolListViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return View(roles);
        }

        [HttpGet]
        public IActionResult RoleCreate()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleCreateViewModel request)
        {
            var result = await _roleManager.CreateAsync(new AppRole() { Name = request.Name });
            if (!result.Succeeded)
            {
                ModelState.AddErrorModelList(result.Errors.Select(x => x.Description).ToList());
                return View();
            }


            return RedirectToAction("Index");
        }




        [HttpGet]
        public async Task<IActionResult> RoleUpdate(string id)
        {
            var roleToUpdate = await _roleManager.FindByIdAsync(id);

            if (roleToUpdate == null)
            {
                throw new Exception("Güncellenecek rol bulunamamaktadır");
            }
            return View(new RoleUpdateViewModel() { Id=roleToUpdate.Id,Name=roleToUpdate.Name});
        }


        [HttpPost]
        public async  Task<IActionResult> RoleUpdate(RoleUpdateViewModel request)
        {
             var rolToUpdate = await _roleManager.FindByIdAsync(request.Id);

            if (rolToUpdate == null)
            {
                throw new Exception("Güncellenecek rol bulunamamaktadır");

            }
            rolToUpdate.Name = request.Name;
            await _roleManager.UpdateAsync(rolToUpdate);

            return RedirectToAction("Index");
        }


        
        public async Task<IActionResult> RoleDelete(string id)
        {
            var roleToDelete = await _roleManager.FindByIdAsync(id);
            if (roleToDelete == null)
            {
                throw new Exception("Silinecek rol bulunamamaktadır");

            }
          var result = await _roleManager.DeleteAsync(roleToDelete);

            if (!result.Succeeded)
            {
                throw new Exception("Silme  İşlemi  gerçekleştirilememiştir.");

            }

            return RedirectToAction("Index");
        }
    }
}
