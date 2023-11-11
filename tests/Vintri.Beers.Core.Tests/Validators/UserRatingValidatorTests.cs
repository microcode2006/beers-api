namespace Vintri.Beers.Core.Tests.Validators;

public class UserRatingValidatorTests
{
    private readonly UserRatingValidator _userRatingValidator = new();
    private readonly Fixture _fixture = new();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Should_Have_Error_When_UserName_Invalid(string userName)
    {
        var userRating = new UserRating
        (
            Username: userName,
            Rating: _fixture.Create<int>()
        );
        var result = _userRatingValidator.TestValidate(userRating);
        result.ShouldHaveValidationErrorFor(userRating => userRating.Username);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(-1)]
    public void Should_Have_Error_When_Rating_Invalid(int rating)
    {
        var userRating = new UserRating
        (
            Username: _fixture.Create<string>(),
            Rating: rating
        );
        var result = _userRatingValidator.TestValidate(userRating);
        result.ShouldHaveValidationErrorFor(userRating => userRating.Rating);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(2)]
    public void Should_Not_Have_Error_When_Rating_Valid(int rating)
    {
        var userRating = new UserRating
        (
            Username: _fixture.Create<string>(),
            Rating: rating
        );
        var result = _userRatingValidator.TestValidate(userRating);
        result.ShouldNotHaveValidationErrorFor(userRating => userRating.Rating);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Should_Not_Have_Error_When_Comments_Valid(string comments)
    {
        var userRating = new UserRating
        (
            Username: _fixture.Create<string>(),
            Rating: _fixture.Create<int>()
        )
        {
            Comments = comments
        };

        var result = _userRatingValidator.TestValidate(userRating);
        result.ShouldNotHaveValidationErrorFor(userRating => userRating.Comments);
    }

    [Fact]
    public void Should_Have_Error_When_Comments_Length_Invalid()
    {
        ValidateComments(51, result => result.ShouldHaveValidationErrorFor(userRating => userRating.Comments));
    }

    [Fact]
    public void Should_Not_Have_Error_When_Comments_Length_Valid()
    {
        ValidateComments(50, result => result.ShouldNotHaveValidationErrorFor(userRating => userRating.Comments));
    }

    private void ValidateComments(int characterCount, Action<TestValidationResult<UserRating>> assertion)
    {

        var chars = _fixture.CreateMany<char>(characterCount).ToArray();

        var userRating = new UserRating
        (
            Username: _fixture.Create<string>(),
            Rating: _fixture.Create<int>()
        )
        {
            Comments = new string(chars)
        };
        var result = _userRatingValidator.TestValidate(userRating);

        assertion(result);
    }

}