using HNBestStories.Interfaces;
using HNBestStories.Models;
using HNBestStories.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HNBestStories.Services
{
    /// <summary>
    /// Story Service implementation responsible for HNStory API comunications.
    /// </summary>
    public class HNStoryService : IStoryService
    {
        private readonly HttpClient httpClient;
        private readonly IOptions<HNStoryServiceSettings> options;

        public HNStoryService(HttpClient httpClient, IOptions<HNStoryServiceSettings> options)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<IEnumerable<string>> GetBestStoriesIds()
        {
            string resource = $"{options.Value.GetStoryIdsResource}";

            var response = await httpClient.GetAsync(new Uri(resource, UriKind.Relative)).ConfigureAwait(false);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<List<string>>(responseContent);
        }

        public async Task<Story> GetStoryDetails(string id)
        {
            string resource = $"{options.Value.GetStoryDetailsResource}{id}.json";

            var response = await httpClient.GetAsync(new Uri(resource, UriKind.Relative)).ConfigureAwait(false);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<Story>(responseContent);
        }
    }
}
