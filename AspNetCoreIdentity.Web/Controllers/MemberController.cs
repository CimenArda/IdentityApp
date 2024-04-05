using AspNetCoreIdentity.Web.Extentions;
using AspNetCoreIdentity.Web.Models;
using AspNetCoreIdentity.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;

namespace AspNetCoreIdentity.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;

        private readonly UserManager<AppUser> _userManager;

        private readonly IFileProvider _fileProvider;

        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
        }

        public async Task<IActionResult> Index() 
        {
            var currentUser = await  _userManager.FindByNameAsync(User.Identity!.Name!);

            var userviewModel =new UserViewModel
            { Email=currentUser!.Email,
                PhoneNumber=currentUser.PhoneNumber
                ,UserName=currentUser.UserName,
                PictureUrl=currentUser.Picture
                        
            };

            return View(userviewModel);
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


            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

            var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser,request.PasswordOld);
            
            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski şifreniz yanlış");
                return View();
            }
            var resultChangePassword = await _userManager.ChangePasswordAsync(currentUser,request.PasswordOld, request.PasswordNewConfirm!);

            if (!resultChangePassword.Succeeded)
            {
                ModelState.AddErrorModelList(resultChangePassword.Errors.Select(x=>x.Description).ToList());
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser,request.PasswordNew!,true,false);


            TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirilmiştir";

            return View();
        }



        public  async Task<IActionResult> UserEdit()
        {
            ViewBag.GenderList = new SelectList(Enum.GetNames(typeof(Gender)));
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var userEditViewModel = new UserEditViewModel()
            {
                UserName=currentUser.UserName,
                BirthDay = currentUser.Birthday.HasValue ? currentUser.Birthday.Value : default(DateTime),
                Email = currentUser.Email,
                Phone=currentUser.PhoneNumber,
                City=currentUser.City,
                Gender=currentUser.Gender,
            };

            return View(userEditViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

            currentUser.UserName= request.UserName;
            currentUser.Email = request.Email;
            currentUser.Birthday = request.BirthDay;
            currentUser.City = request.City;
            currentUser.Gender= request.Gender;
            currentUser.PhoneNumber = request.Phone;


            if (request.Picture != null && request.Picture.Length>0)
            {
                var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");
                
                var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.Picture.FileName)}"; //.jpg .png


                var newPicturePath =Path.Combine(wwwrootFolder.First(x=>x.Name =="UserPicture").PhysicalPath!,randomFileName);

                using var stream = new FileStream(newPicturePath, FileMode.Create);
                await request.Picture.CopyToAsync(stream);

                currentUser.Picture = randomFileName;

            }
               var updateToUserResult=await _userManager.UpdateAsync(currentUser);


            if (!updateToUserResult.Succeeded)
            {
                ModelState.AddErrorModelList(updateToUserResult.Errors.Select(x=>x.Description).ToList());
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(currentUser,true);

            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser.UserName,
                BirthDay = currentUser.Birthday.HasValue ? currentUser.Birthday.Value : default(DateTime),
                Email = currentUser.Email,
                Phone = currentUser.PhoneNumber,
                City = currentUser.City,
                Gender = currentUser.Gender,
               
            };
            TempData["SuccessMessage"] = "Kullanıcı Bilgileriniz Başarıyla  değiştirilmiştir";
            return View(userEditViewModel);
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

        public async Task<IActionResult> Logout()
        {
            await  _signInManager.SignOutAsync();

            return RedirectToAction("Index","Home");
        }


    }
}
