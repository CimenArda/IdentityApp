﻿using AspNetCoreIdentityRepository.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNetCoreIdentity.Web.ClaimProvider
{
    public class UserClaimProvider : IClaimsTransformation
    {

        private readonly UserManager<AppUser>  _userManager;

        public UserClaimProvider(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identityUser = principal.Identity as ClaimsIdentity;


            var currentUser = await _userManager.FindByNameAsync(identityUser!.Name!);
        
            if (String.IsNullOrEmpty(currentUser.City))
            {
                return principal;
            }

           
             if (principal.HasClaim(x => x.Type != "City"))
             {
                    Claim cityClaim = new Claim("City", currentUser.City);
                    identityUser.AddClaim(cityClaim);

             }

            
                return principal;

        }
    }
}
