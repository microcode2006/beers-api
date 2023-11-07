namespace Vintri.Beers.Core.Tests.Services;

public class BeersServiceTests
{
    private readonly BeersService _beersService;
    private readonly IUserRatingRepository _userRatingRepository = Substitute.For<IUserRatingRepository>();
    private readonly IPunkClient _punkClient = Substitute.For<IPunkClient>();
    private readonly IValidator<BeerRating> _beerRatingValidator = Substitute.For<IValidator<BeerRating>>();
    private readonly IValidator<QueryFilter> _queryFilterValidator = Substitute.For<IValidator<QueryFilter>>();

    public BeersServiceTests()
    {
        _beersService = new BeersService(_userRatingRepository, _punkClient, _beerRatingValidator, _queryFilterValidator);
    }

    [Fact]
    public async Task AddRatingAsync_Should_Add_Rating()
    {
        var beerRating = new BeerRating();
        await _beersService.AddRatingAsync(beerRating, default);

        beerRating.ShouldSatisfyAllConditions(
            rating => _userRatingRepository.Received(1).AddAsync(rating, default),
            rating => _beerRatingValidator.Received(1).ValidateAndThrowAsync(rating)
        );
    }

    // TODO: All GetBeersAsync test data could be reorganized by MemberData attribute
    [Fact]
    public async Task GetBeersAsync_Should_Return_BeerRatingsResponse()
    {
        var beers = new List<Beer>
        {
            new()
            {
                Id = 1,
                Name = "Name",
                Description = "Description",
            },
            new()
            {
                Id = 2,
                Name = "Name2",
                Description = "Description2",
            }

        };
        var beerRatings = new List<BeerRatings>
        {
            new()
            {
                Id = 1,
                UserRatings = new List<UserRating>
                {
                    new()
                    {
                        Username = "test@vintri.ca",
                        Rating = 1,
                        Comments = "good",
                    },
                    new()
                    {
                        Username = "test2@vintri.ca",
                        Rating = 2,
                        Comments = "very good",
                    },
                }
            }

        };

        var expectedBeerRatingsResponse = new List<BeerRatingsResponse>
        {
            new()
            {
                Id = 1,
                Name = "Name",
                Description = "Description",
                UserRatings = new List<UserRating>
                {
                    new()
                    {
                        Username = "test@vintri.ca",
                        Rating = 1,
                        Comments = "good",
                    },
                    new()
                    {
                        Username = "test2@vintri.ca",
                        Rating = 2,
                        Comments = "very good",
                    },
                }
            },
            new()
            {
                Id = 2,
                Name = "Name2",
                Description = "Description2",
                UserRatings = new List<UserRating>()
            }
        };

        var queryFilter = new QueryFilter();

        _punkClient.GetBeersAsync(default, default).ReturnsForAnyArgs(Task.FromResult((IReadOnlyList<Beer>)beers));
        _userRatingRepository.GetAsync(Arg.Is<List<int>>(x => x.Contains(1) && x.Contains(2)), default)
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
        var beers = new List<Beer>
        {
            new()
            {
                Id = 1,
                Name = "Name",
                Description = "Description",
            }
        };
        var beerRatings = new List<BeerRatings>
        {
            new()
            {
                Id = 2,
                UserRatings = new List<UserRating>
                {
                    new()
                    {
                        Username = "test@vintri.ca",
                        Rating = 1,
                        Comments = "good",
                    }
                }
            }

        };

        var expectedBeerRatingsResponse = new List<BeerRatingsResponse>
        {
            new()
            {
                Id = 1,
                Name = "Name",
                Description = "Description",
                UserRatings = new List<UserRating>()
            }
        };

        _punkClient.GetBeersAsync(default, default).ReturnsForAnyArgs(Task.FromResult((IReadOnlyList<Beer>)beers));
        _userRatingRepository.GetAsync(Arg.Is<List<int>>(x => x.Contains(1)), default)
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
        _userRatingRepository.GetAsync(default!, default)
            .ReturnsForAnyArgs(Task.FromResult((IReadOnlyList<BeerRatings>)beerRatings));

        var actualResult = await _beersService.GetBeersAsync(default!, default);

        actualResult.Count.ShouldBe(0);

    }
}