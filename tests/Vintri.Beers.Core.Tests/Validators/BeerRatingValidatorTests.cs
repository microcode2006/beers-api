namespace Vintri.Beers.Core.Tests.Validators;

public class BeerRatingValidatorTests
{
    private readonly IPunkClient _mockedPunkClient = Substitute.For<IPunkClient>();
    private readonly BeerRatingValidator _validator;

    public BeerRatingValidatorTests()
    {
        _validator = new BeerRatingValidator(_mockedPunkClient);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Should_Have_Error_When_BeerId_Invalid(int beerId)
    {
        var beerRating = new BeerRating
        {
            BeerId = beerId
        };

        var result = await _validator.TestValidateAsync(beerRating);
        result.ShouldHaveValidationErrorFor(beerRating => beerRating.BeerId);
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_BeerId_Exists()
    {
        const int existingBeerId = 1;
        var beerRatingUnderTest = new BeerRating { BeerId = existingBeerId };
        var beers = new List<Beer> { new() };

        SetBeerMocks(beers, existingBeerId);

        var validationResult = await _validator.TestValidateAsync(beerRatingUnderTest);
        validationResult.ShouldNotHaveValidationErrorFor(beerRating => beerRating.BeerId);
    }

    [Fact]
    public async Task Should_Have_Error_When_BeerId_Not_Exists()
    {
        var beerRatingUnderTest = new BeerRating();
        var emptyBeerList = new List<Beer>();

        SetBeerMocks(emptyBeerList);

        var validationResult = await _validator.TestValidateAsync(beerRatingUnderTest);
        validationResult.ShouldHaveValidationErrorFor(beerRating => beerRating.BeerId);
    }

    private void SetBeerMocks(IReadOnlyList<Beer> beers, OneOf<int, QueryFilter> filter = default)
    {
        _mockedPunkClient
            .GetBeersAsync(filter, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(beers));
    }
}