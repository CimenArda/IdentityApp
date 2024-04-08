using AspNetCoreIdentity.Web.Extentions;
using AspNetCoreIdentityRepository.Models;
using AspNetCoreIdentityApp.Core.Models;
using AspNetCoreIdentityApp.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using AspNetCoreIdentityService.Services;

namespace AspNetCoreIdentity.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;

        private readonly UserManager<AppUser> _userManager;

        private readonly IFileProvider _fileProvider;

        private  string userName => User.Identity!.Name!;
        private readonly IMemberService _memberService;

        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider, IMemberService memberService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
            _memberService = memberService;
        }

        public async Task<IActionResult> Index() 
        {
           

            return View(await _memberService.GetUserViewModelByUserName(userName));
        }





        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }


            if (! await _memberService.CheckPasswordAsync(userName,request.PasswordOld))
            {
                ModelState.AddModelError(string.Empty, "Eski şifreniz yanlış");
                return View();
            }
            var (isSuccess,errors) = await _memberService.ChangePasswordAsync(userName,request.PasswordOld,request.PasswordNew!);


            if (!isSuccess)
            {
                ModelState.AddIdentityErrors(errors);
                return View();
            }

          
            TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirilmiştir";

            return View();
        }



        public  async Task<IActionResult> UserEdit()
        {
            ViewBag.GenderList = new SelectList(Enum.GetNames(typeof(Gender)));
           

            return View(await _memberService.GetUserEditViewModelAsync(userName));
        }


        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var  (isSuccess,errors) = await _memberService.EditUserAsync(request, userName);



            if (!isSuccess)
            {
                ModelState.AddIdentityErrors(errors);
                return View();
            }

          
           
            TempData["SuccessMessage"] = "Kullanıcı Bilgileriniz Başarıyla  değiştirilmiştir";
            return View(await _memberService.GetUserEditViewModelAsync(userName));
        }




        public async Task<IActionResult> AccessDenied(string ReturnUrl)
        {
            return View();
        }




        [HttpGet]
        public async  Task<IActionResult> Claims()
        {

            var userClaimList = User.Claims.Select(x => new ClaimViewModel()
            {
                Issuer = x.Issuer,
                Type = x.Type,
                Value = x.Value
            }).ToList();

            return View(userClaimList);
        }


        [Authorize(Policy ="AnkaraPolicy")]
        [HttpGet]
        public IActionResult AnkaraPage()
        {
            return View();
        }
        [Authorize(Policy = "ExchangePolicy")]
        [HttpGet]
        public IActionResult ExchangePage()
        {
            return View();
        }


        [Authorize(Policy = "VioloncePolicy")]
        [HttpGet]
        public IActionResult VioloncePage()
        {
            return View();
        }

        public  IActionResult Logout()
        {
            
            _memberService.Logout();
            return RedirectToAction("Index","Home");
        }


    }
}
