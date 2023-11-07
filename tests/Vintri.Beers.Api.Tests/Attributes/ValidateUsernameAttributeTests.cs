using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using Vintri.Beers.Api.Attributes;

namespace Vintri.Beers.Api.Tests.Attributes;

public class ValidateUsernameAttributeTests
{
    private readonly ValidateUsernameAttribute _attribute = new();

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("username")]
    [InlineData("username@")]
    [InlineData("username@.com")]
    [InlineData("@.com")]
    [InlineData("username @test.com")]
    [InlineData("username@test.com", false)]
    public void OnActionExecuting_Should_Return_BadRequest_When_InvalidEmail(string username, bool isBadRequest = true)
    {
        var actionContext = new HttpActionContext
        {
            ControllerContext = new HttpControllerContext
            {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage()
            },
            ActionArguments = { ["userRating"] = new UserRating { Username = username }},
            ActionDescriptor = Substitute.For<HttpActionDescriptor>()
        };
        actionContext.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

        _attribute.OnActionExecuting(actionContext);

        if (isBadRequest)
        {
            actionContext.Response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
    }
}