namespace Vintri.Beers.Api.Tests.Controllers;

public class BeersControllerTests
{
    private readonly IBeersService _beersService = Substitute.For<IBeersService>();
    private readonly Fixture _fixture = new();
    private readonly BeersController _beersController;

    public BeersControllerTests()
    {
        _beersController = new BeersController(_beersService);
        _beersController.Configuration = new HttpConfiguration();
    }

    [Fact]
    public async Task GetBeersAsync_Should_Return_Ok_Result()
    {
        _beersController.Request = new HttpRequestMessage(HttpMethod.Get, string.Empty);
        var expectedBeerRatingsResponse = _fixture.Create<List<BeerRatingsResponse>>();
        var beerName = _fixture.Create<string>();

        _beersService.GetBeersAsync(Arg.Is<QueryFilter>(x => x.BeerName == beerName), default).Returns(
            Task.FromResult((IReadOnlyList<BeerRatingsResponse>)expectedBeerRatingsResponse));

        var httpActionResult = await _beersController.GetBeersAsync(new QueryFilter{ BeerName = beerName });
        var result = httpActionResult.ExecuteAsync(default).Result;

        result.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.TryGetContentValue<List<BeerRatingsResponse>>(out var actualBeerRatingsResponse).ShouldBeTrue();
        actualBeerRatingsResponse.ShouldBeEquivalentTo(expectedBeerRatingsResponse);
    }

    [Fact]
    public async Task AddRatingAsync_Should_Return_Created_Result()
    {
        _beersController.Request = new HttpRequestMessage(HttpMethod.Post, string.Empty);
        var expectedUserRating = _fixture.Create<UserRating>();
        var beerId = _fixture.Create<int>();

        var httpActionResult = await _beersController.AddRatingAsync(beerId, expectedUserRating);
        var result = httpActionResult.ExecuteAsync(default).Result;

        await _beersService.Received(1).AddRatingAsync(Arg.Is<BeerRating>(x => x.BeerId == beerId && x.UserRating == expectedUserRating), default);
        result.StatusCode.ShouldBe(HttpStatusCode.Created);
        result.TryGetContentValue<BeerRating>(out var actualBeerRating).ShouldBeTrue();
        actualBeerRating.UserRating.ShouldBeEquivalentTo(expectedUserRating);
        actualBeerRating.BeerId.ShouldBe(beerId);
    }
}