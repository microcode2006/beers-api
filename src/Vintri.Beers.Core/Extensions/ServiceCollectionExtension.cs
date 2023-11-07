using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Vintri.Beers.Core.Interfaces;
using Vintri.Beers.Core.Models;
using Vintri.Beers.Core.Services;
using Vintri.Beers.Core.Validators;

namespace Vintri.Beers.Core.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddValidators(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IValidator<UserRating>, UserRatingValidator>();
        serviceCollection.AddScoped<IValidator<BeerRating>, BeerRatingValidator>();
        serviceCollection.AddScoped<IValidator<QueryFilter>, QueryFilterValidator>();
    }

    public static void AddBeersService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IBeersService, BeersService>();
    }


}