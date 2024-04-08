using AspNetCoreIdentityApp.Core.ViewModels;
using AspNetCoreIdentityRepository.Models;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
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
        private readonly IFileProvider _fileProvider;

        public MemberService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IFileProvider fileProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileProvider = fileProvider;
        }

        public async Task<(bool, IEnumerable<IdentityError>?)> ChangePasswordAsync(string userName, string oldPassword, string newPassword)
        {
            var currentUser = await _userManager.FindByNameAsync(userName);

            var resultChangePassword = await _userManager.ChangePasswordAsync(currentUser, oldPassword, newPassword);

            if (!resultChangePassword.Succeeded)
            {
                return (false, resultChangePassword.Errors);
            }
            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser, newPassword, true, false);

            return (true, null);

        }

        public async Task<bool> CheckPasswordAsync(string userName, string password)
        {
            var currentUser = await _userManager.FindByNameAsync(userName);

              return  await _userManager.CheckPasswordAsync(currentUser, password);

        }

        public async Task<(bool, IEnumerable<IdentityError>?)> EditUserAsync(UserEditViewModel request, string userName)
        {
            var currentUser = await _userManager.FindByNameAsync(userName);

            currentUser.UserName = request.UserName;
            currentUser.Email = request.Email;
            currentUser.Birthday = request.BirthDay;
            currentUser.City = request.City;
            currentUser.Gender = request.Gender;
            currentUser.PhoneNumber = request.Phone;


            if (request.Picture != null && request.Picture.Length > 0)
            {
                var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");

                var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.Picture.FileName)}"; //.jpg .png


                var newPicturePath = Path.Combine(wwwrootFolder.First(x => x.Name == "UserPicture").PhysicalPath!, randomFileName);

                using var stream = new FileStream(newPicturePath, FileMode.Create);
                await request.Picture.CopyToAsync(stream);

                currentUser.Picture = randomFileName;

            }

            var updateToUserResult = await _userManager.UpdateAsync(currentUser);
            if (!updateToUserResult.Succeeded)
            {
                return (false, updateToUserResult.Errors);
            }
            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();


            await _signInManager.SignInAsync(currentUser, true);

          
            return (true, null);

        }

        public async Task<UserEditViewModel> GetUserEditViewModelAsync(string userName)
        {
            var currentUser = await _userManager.FindByNameAsync(userName);
           return new UserEditViewModel()
            {
                UserName = currentUser.UserName,
                BirthDay = currentUser.Birthday.HasValue ? currentUser.Birthday.Value : default(DateTime),
                Email = currentUser.Email,
                Phone = currentUser.PhoneNumber,
                City = currentUser.City,
                Gender = currentUser.Gender,
            };
        }

        public async Task<UserViewModel> GetUserViewModelByUserName(string userName)
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
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

       
       

       


    

    }
}
