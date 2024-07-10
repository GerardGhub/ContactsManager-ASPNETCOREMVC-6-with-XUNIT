using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.AuthorizationFilter
{
    // Custom authorization filter to check for a specific authentication token in cookies
    public class TokenAuthorizationFilter : IAuthorizationFilter
    {
        // This method is called when authorization is needed
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Check if the "Auth-Key" cookie exists in the request
            if (context.HttpContext.Request.Cookies.ContainsKey("Auth-Key") == false)
            {
                // If the "Auth-Key" cookie is not present, set the result to 401 Unauthorized
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return; // Exit the method early
            }

            // Check if the "Auth-Key" cookie's value matches the expected value "A100"
            if (context.HttpContext.Request.Cookies["Auth-Key"] != "A100")
            {
                // If the cookie value does not match, set the result to 401 Unauthorized
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
        }
    }
}
