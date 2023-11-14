namespace Vintri.Beers.Core.Tests.Services;

public class BeersServiceTests
{
    private readonly BeersService _beersService;
    private readonly Fixture _fixture = new();
    private readonly IBeerRatingRepository _beerRatingRepository = Substitute.For<IBeerRatingRepository>();
    private readonly IPunkClient _punkClient = Substitute.For<IPunkClient>();
    private readonly IValidator<BeerRating> _beerRatingValidator = Substitute.For<IValidator<BeerRating>>();
    private readonly IValidator<QueryFilter> _queryFilterValidator = Substitute.For<IValidator<QueryFilter>>();

    public BeersServiceTests()
    {
        _beersService = new BeersService(_beerRatingRepository, _punkClient, _beerRatingValidator, _queryFilterValidator);
    }

    [Fact]
    public async Task AddRatingAsync_Should_Add_Rating()
    {
        var beerRating = _fixture.Create<BeerRating>();
        await _beersService.AddRatingAsync(beerRating, default);

        beerRating.ShouldSatisfyAllConditions(
            rating => _beerRatingRepository.Received(1).AddAsync(rating, default),
            rating => _beerRatingValidator.Received(1).ValidateAndThrowAsync(rating)
        );
    }

    // TODO: All GetBeersAsync test data could be reorganized by MemberData attribute
    [Fact]
    public async Task GetBeersAsync_Should_Return_BeerRatingsResponse()
    {
        var userRatings = _fixture.Create<List<UserRating>>();
        var beer = (name1: _fixture.Create<string>(), name2: _fixture.Create<string>(),
            description1: _fixture.Create<string>(), description2: _fixture.Create<string>());

        var beers = new List<Beer>
        {
            new(Id: 1, Name: beer.name1, Description: beer.description1),
            new(Id: 2, Name: beer.name2, Description: beer.description2)

        };
        var beerRatings = new List<BeerRatings>
        {
            new (Id: 1, UserRatings: userRatings)
        };

        var expectedBeerRatingsResponse = new List<BeerRatingsResponse>
        {
            new(Id: 1, Name: beer.name1, Description: beer.description1, UserRatings: userRatings),
            new(Id: 2, Name: beer.name2, Description: beer.description2, UserRatings: new List<UserRating>())
        };

        var queryFilter = new QueryFilter{ BeerName = _fixture.Create<string>() };

        _punkClient.GetBeersAsync(default, default).ReturnsForAnyArgs(Task.FromResult((IReadOnlyList<Beer>)beers));
        _beerRatingRepository.GetAsync(Arg.Is<HashSet<int>>(x => x.Contains(1) && x.Contains(2)), default)
            .Returns(Task.FromResult((IReadOnlyList<BeerRatings>)beerRatings));

        var actualResult = await _beersService.GetBeersAsync(queryFilter, default);

        queryFilter.ShouldSatisfyAllConditions(
            _ => actualResult.ShouldBeEquivalentTo(expectedBeerRatingsResponse),
            filter => _queryFilterValidator.Received(1).ValidateAndThrowAsync(filter)
        );
    }

    [Fact]
    public async Task GetBeersAsync_Should_Return_Empty_BeerRatingsResponse_When_No_Matched_BeerIds()
    {
        var beer = (name: _fixture.Create<string>(), description: _fixture.Create<string>());

        var beers = new List<Beer>
        {
            new(Id: 1, Name: beer.name, Description: beer.description)
        };
        var beerRatings = new List<BeerRatings>
        {
            new(Id: 2, UserRatings: _fixture.Create<List<UserRating>>())
        };

        var expectedBeerRatingsResponse = new List<BeerRatingsResponse>
        {
            new(Id: 1, Name: beer.name, Description: beer.description, UserRatings: new List<UserRating>())
        };

        _punkClient.GetBeersAsync(default, default).ReturnsForAnyArgs(Task.FromResult((IReadOnlyList<Beer>)beers));
        _beerRatingRepository.GetAsync(Arg.Is<HashSet<int>>(x => x.Contains(1)), default)
            .Returns(Task.FromResult((IReadOnlyList<BeerRatings>)beerRatings));

        var actualResult = await _beersService.GetBeersAsync(default!, default);

        actualResult.ShouldBeEquivalentTo(expectedBeerRatingsResponse);

    }

    [Fact]
    public async Task GetBeersAsync_Should_Return_Empty_BeerRatingsResponse_When_No_Beers()
    {
        var beers = new List<Beer>();
        var beerRatings = new List<BeerRatings>();

        _punkClient.GetBeersAsync(default, default).ReturnsForAnyArgs(Task.FromResult((IReadOnlyList<Beer>)beers));
        _beerRatingRepository.GetAsync(default!, default).ReturnsForAnyArgs(Task.FromResult((IReadOnlyList<BeerRatings>)beerRatings));

        var actualResult = await _beersService.GetBeersAsync(default!, default);

        actualResult.Count.ShouldBe(0);

    }
}