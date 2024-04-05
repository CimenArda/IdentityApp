using AspNetCoreIdentity.Web.Extentions;
using AspNetCoreIdentity.Web.Models;
using AspNetCoreIdentity.Web.Services;
using AspNetCoreIdentity.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace AspNetCoreIdentity.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;

        private readonly IEmailService _emailservice;


        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,IEmailService emailService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailservice = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }




        public IActionResult Signin()
        {



            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Signin(SignInViewModel model, string? returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Action("Index", "Home");

            var hasUser = await _userManager.FindByEmailAsync(model.Email!);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya Şifreniz Yanlış.");
                return View();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, model.Password!, model.RememberMe, true);

            if (signInResult.IsLockedOut)
            {
                ModelState.AddErrorModelList(new List<string>() { "3 dakika boyunca giriş yapamazsınız." });
                return View();
            }

            if (!signInResult.Succeeded)
            {
                ModelState.AddErrorModelList(new List<string>() { $"Email veya şifreniz yanlış.Başarısız giriş sayısı:{await _userManager.GetAccessFailedCountAsync(hasUser)}" });
                return View();
            }

            if (hasUser.Birthday.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(hasUser, model.RememberMe, new[] { new Claim("Birthday", hasUser.Birthday.Value.ToString()) });
            }
            return Redirect(returnUrl!);


        }




        public IActionResult Signup()
        {



            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupViewModel requests)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var identityResult = await _userManager.CreateAsync(new() { UserName = requests.UserName, PhoneNumber = requests.Phone, Email = requests.Email }, requests.PasswordConfirm);


            if (!identityResult.Succeeded)
            {
                ModelState.AddErrorModelList(identityResult.Errors.Select(x => x.Description).ToList());
                return View();

            }
          
                var exchangeExpireClaim = new Claim("ExchangeExpireDate",DateTime.Now.AddDays(10).ToString());

                var user = await _userManager.FindByNameAsync(requests.UserName);

               var claimResult =  await _userManager.AddClaimAsync(user!, exchangeExpireClaim);

            if (!claimResult.Succeeded)
            {

                ModelState.AddErrorModelList(identityResult.Errors.Select(x => x.Description).ToList());
                return View();
            }
                TempData["SuccessMessage"] = "Üyelik Kayıt işlemi başarıyla gerçekleşmiştir.";
                return RedirectToAction(nameof(HomeController.Signup));//get kısmına bir daha istek
            
                
         


         



        }





        public IActionResult ForgetPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
        {
            var hasUser = await _userManager.FindByEmailAsync(request.Email);

            if (hasUser==null)
            {
                return RedirectToAction("Signin","Home");
            }

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);

            var passwordResetLink = Url.Action("ResetPassword", "Home", new { userId = hasUser.Id, Token = passwordResetToken },HttpContext.Request.Scheme);
            // örnek link https://localhost://7155?userID=12213&token=adawwqeqwdsazcx


            //Email Service

            await _emailservice.SendResetPasswordEmail(passwordResetLink!,hasUser.Email!);





            TempData["SuccessMessage"] = "Şifre yenileme linki e-posta adresinize gönderilmiştir.";
            return RedirectToAction(nameof(ForgetPassword));









        }



        public IActionResult ResetPassword(string userId,string token)
        {
            //otomatik olarak mappleniyor
            
            TempData["userId"]=userId;
            TempData["token"]=token;
           


            return View();

        }


        [HttpPost]
        public async Task <IActionResult> ResetPassword(ResetPasswordViewModel request)
        {
            var userId = TempData["userId"].ToString();
            var token = TempData["token"].ToString();

            if (userId==null || token==null)
            {
                throw new Exception("Bir hata meydana geldi.");
            }

            var hasUser = await _userManager.FindByIdAsync(userId);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamamıştır");
            }

            var result = await _userManager.ResetPasswordAsync(hasUser, token, request.Password);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Şifreniz başarıyla yenilenmiştir";
            }
            else
            {
                ModelState.AddErrorModelList(result.Errors.Select(x=>x.Description).ToList());
                
            }

                return View();


        }











        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
