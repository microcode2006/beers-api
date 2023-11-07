using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Vintri.Beers.Core.Models;

namespace Vintri.Beers.Api.Attributes
{
    /// <summary>
    /// Here's the username validation only, other validation is done through separate fluent validation.
    /// </summary>
    public class ValidateUsernameAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ActionArguments.TryGetValue("userRating", out var argument))
            {
                if (argument is UserRating userRating && !IsValidEmail(userRating.Username))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(
                        HttpStatusCode.BadRequest, $"Please specify a valid email for {nameof(userRating.Username)}");
                }
            }
        }

        private static bool IsValidEmail(string email) => Regex.IsMatch(email ?? string.Empty, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
    }
}