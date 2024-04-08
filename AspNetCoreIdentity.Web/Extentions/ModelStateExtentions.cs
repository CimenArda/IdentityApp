using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCoreIdentity.Web.Extentions
{
    public static class ModelStateExtentions
    {
        public static void AddErrorModelList(this ModelStateDictionary modelState,List<string> errors)
        {
            errors.ForEach(x => {

                modelState.AddModelError(string.Empty, x);
            });


           
        }

        public static void AddIdentityErrors(this ModelStateDictionary modelState, IEnumerable<IdentityError> errors)
        {
            foreach (var error in errors)
            {
                modelState.AddModelError(string.Empty, error.Description);
            }
        }


    }
}
