using System.Net;
using System.Text.RegularExpressions;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Vintri.Beers.Core.Exceptions;
using Vintri.Beers.Core.Extensions;
using Vintri.Beers.Core.Models;

namespace Vintri.Beers.Api.Attributes
{
    /// <inheritdoc />
    public class ValidateUsernameAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Here's the username validation only, other validation is done through separate validators with fluent validation.
        /// Its probably better to have all model validations in separate validators consumed by service class as part of business logic.
        /// Its also easier for unit test without http action context etc.
        /// </summary>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ActionArguments.TryGetValue("userRating", out var argument))
            {
                if (argument is UserRating userRating && !IsValidEmail(userRating.Username))
                {
                    throw new InvalidUsernameException($"Invalid email for {nameof(userRating.Username)}");
                }
            }

            return;

            bool IsValidEmail(string? email) => Regex.IsMatch(email ?? string.Empty, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
        }

    }
}