using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Vintri.Beers.Core.Interfaces;
using Vintri.Beers.Core.Models;
using OneOf;

namespace Vintri.Beers.Infrastructure
{
    public class PunkClient : IPunkClient
    {
        private readonly HttpClient _httpClient;
        private readonly PunkClientSettings _punkClientSettings;

        public PunkClient(HttpClient httpClient, IOptions<PunkClientSettings> punkClientSettings)
        {
            _httpClient = httpClient;
            _punkClientSettings = punkClientSettings.Value;
        }

        public async Task<IReadOnlyList<Beer>> GetBeersAsync(OneOf<int, QueryFilter> queryOption, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var requestUri = queryOption.IsT0 ?
                $"{_punkClientSettings.Endpoint}/{queryOption.AsT0.ToString()}" :
                $"{_punkClientSettings.Endpoint}?{GetQueryString(queryOption.AsT1)}";

            var response = await _httpClient.GetAsync(requestUri , cancellationToken).ConfigureAwait(false);
            var beers = await GetBeersFromResponseAsync(response).ConfigureAwait(false);

            return beers;
        }

        private NameValueCollection GetQueryString(QueryFilter queryFilter)
        {
            var queryStrings = HttpUtility.ParseQueryString(string.Empty);
            queryStrings["beer_name"] = queryFilter.BeerName;
            queryStrings["page"] = queryFilter.Page.ToString();
            queryStrings["per_page"] = queryFilter.PerPage.ToString();

            return queryStrings;
        }

        private async Task<IReadOnlyList<Beer>> GetBeersFromResponseAsync(HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<Beer>>(responseContent);
                case HttpStatusCode.NotFound:
                    return Array.Empty<Beer>();
                default:
                    throw new HttpRequestException(response.ReasonPhrase);
            }
        }
    }
}