using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Threading;
using Vintri.Beers.Core.Models;
using Vintri.Beers.Core.Interfaces;

namespace Vintri.Beers.Infrastructure.Repositories;

/// <summary>
/// Json file repository for user ratings, use System.Text.Json library for async and performant operations
/// </summary>
internal class BeerRatingRepository : IBeerRatingRepository
{
    private static readonly string DatabaseFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "database.json");

    public async Task AddAsync(BeerRating beerRating, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var beerRatingsList = await LoadBeerRatingsFromFileAsync(cancellationToken).ConfigureAwait(false);
        UpdateBeerRatingsList(beerRating, beerRatingsList);
        await SaveBeerRatingsToFileAsync(beerRatingsList, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<BeerRatings>> GetAsync(HashSet<int> beerIds, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if(!beerIds.Any())
        {
            return new List<BeerRatings>();
        }

        var beerRatingsList = await LoadBeerRatingsFromFileAsync(cancellationToken).ConfigureAwait(false);
        return beerRatingsList.Where(beerRatings => beerIds.Contains(beerRatings.Id)).ToList();
    }

    private static async Task<List<BeerRatings>> LoadBeerRatingsFromFileAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(DatabaseFile) || new FileInfo(DatabaseFile).Length == 0)
        {
            return new List<BeerRatings>();
        }

        //File.OpenRead has a default FileShare value of Read to allow subsequent opening of the file for reading
        using var json = File.OpenRead(DatabaseFile);

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var beerRatings = await JsonSerializer.DeserializeAsync<List<BeerRatings>>(json, options, cancellationToken)
            .ConfigureAwait(false);

        return beerRatings ?? new List<BeerRatings>();
    }

    private static void UpdateBeerRatingsList(BeerRating beerRating, List<BeerRatings> beerRatingsList)
    {
        var existingBeerRating = beerRatingsList.FirstOrDefault(x => x.Id == beerRating.BeerId);

        if (existingBeerRating is null)
        {
            beerRatingsList.Add(
                new BeerRatings
                (
                    Id: beerRating.BeerId,
                    UserRatings: new List<UserRating>
                    {
                        beerRating.UserRating
                    }
                ));
        }
        else
        {
            existingBeerRating.UserRatings.Add(beerRating.UserRating);
        }
    }

    private static async Task SaveBeerRatingsToFileAsync(List<BeerRatings> beerRatingsList, CancellationToken cancellationToken)
    {
        // File.Create has a default FileShare value of None, no other process can access the created file until the file is closed
        using var databaseFileStream = File.Create(DatabaseFile, 4096, FileOptions.Asynchronous);
        await JsonSerializer.SerializeAsync(utf8Json: databaseFileStream, value: beerRatingsList,
            cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}