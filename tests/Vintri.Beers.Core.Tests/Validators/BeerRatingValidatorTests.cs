namespace Vintri.Beers.Core.Tests.Validators;

public class BeerRatingValidatorTests
{
    private readonly IPunkClient _punkClient = Substitute.For<IPunkClient>();
    private readonly BeerRatingValidator _beerRatingValidator;
    private readonly Fixture _fixture = new();

    public BeerRatingValidatorTests()
    {
        _beerRatingValidator = new BeerRatingValidator(_punkClient);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Should_Have_Error_When_BeerId_Invalid(int beerId)
    {
        var beerRating = new BeerRating(beerId, _fixture.Create<UserRating>());

        var result = await _beerRatingValidator.TestValidateAsync(beerRating);
        result.ShouldHaveValidationErrorFor(beerRating => beerRating.BeerId);
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_BeerId_Exists()
    {
        var existingBeerId = _fixture.Create<int>();
        var beerRatingUnderTest = new BeerRating(existingBeerId, _fixture.Create<UserRating>());
        var beers = _fixture.Create<List<Beer>>();

        _punkClient.GetBeersAsync(existingBeerId, Arg.Any<CancellationToken>()).Returns(beers);

        var validationResult = await _beerRatingValidator.TestValidateAsync(beerRatingUnderTest);
        validationResult.ShouldNotHaveValidationErrorFor(beerRating => beerRating.BeerId);
    }

}