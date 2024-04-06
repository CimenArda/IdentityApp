using AspNetCoreIdentityApp.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreIdentityService.Services
{
    public interface IMemberService
    {
        Task<UserViewModel> GetUserViewModelByUserName(string userName);
        Task Logout();
    }
}
