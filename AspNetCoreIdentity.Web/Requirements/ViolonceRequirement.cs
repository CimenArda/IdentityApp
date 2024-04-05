using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreIdentity.Web.Requirements
{
    public class ViolonceRequirement :IAuthorizationRequirement
    {
        public int ThresholdAge { get; set; }
    }

    public class ViolonceExpireRequirementHandler : AuthorizationHandler<ViolonceRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViolonceRequirement requirement)
        {
            if (!context.User.HasClaim(x => x.Type == "Birthday"))
            {
                context.Fail();
                return Task.CompletedTask;

            }

            var birthdateClaim = context.User.FindFirst("Birthday")!;

            var today =DateTime.Now;
            var birthdate = Convert.ToDateTime(birthdateClaim.Value);
            var age =today.Year - birthdate.Year ;

            if (birthdate > today.AddYears(-age)) age--;
        



            if (requirement.ThresholdAge>age)
            {
                context.Fail();
                return Task.CompletedTask;

            }

            context.Succeed(requirement);
            return Task.CompletedTask;


        }
    }

}
