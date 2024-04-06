using AspNetCoreIdentityApp.Core.ViewModels;
using AspNetCoreIdentityRepository.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreIdentityService.Services
{
    public class MemberService :IMemberService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public MemberService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        async   Task<UserViewModel> GetUserViewModelByUserName(string userName)
        {
            var currentUser = await _userManager.FindByNameAsync(userName);

            return new UserViewModel
            {
                Email = currentUser!.Email,
                PhoneNumber = currentUser.PhoneNumber
              ,
                UserName = currentUser.UserName,
                PictureUrl = currentUser.Picture

            };
        };

        Task<UserViewModel> IMemberService.GetUserViewModelByUserName(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
