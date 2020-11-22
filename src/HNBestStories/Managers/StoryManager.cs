using AutoMapper;
using HNBestStories.Interfaces;
using HNBestStories.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HNBestStories.Managers
{
    /// <summary>
    /// The story manager responsible for business layer logic.
    /// </summary>
    public class StoryManager : IStoryManager
    {
        private readonly MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(2));

        /// <summary>
        /// The story manager
        /// </summary>
        private readonly IStoryService storyService;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// The memory cache
        /// </summary>
        private readonly IMemoryCache cache;

        /// <summary>
        /// The logger provider.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Story Manager constructor.
        /// </summary>
        /// <param name="storyService"></param>
        /// <param name="mapper"></param>
        /// <param name="cache"></param>
        public StoryManager(IStoryService storyService, IMapper mapper, IMemoryCache cache, ILogger<StoryManager> logger)
        {
            this.storyService = storyService ?? throw new ArgumentNullException(nameof(storyService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<StoryDTO>> GetBestStories()
        {
            IEnumerable<string> storyIds;

            try
            {
                //Get the top story ids from story provider
                storyIds = await storyService.GetBestStoriesIds();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting best stry ids from story service!", ex);
                throw;
            }


            if (storyIds == null || !storyIds.Any())
            {
                _logger.LogInformation("No StoryIds returned from Story Service!");
                return null;
            }

            //Assuming the stry api returns ids already ordered by score take only the first 20 and throw in there a distinct just to guarantee unique stories
            var topStoryIds = storyIds.Distinct().Take(20);

            // Get unique string to validate if all story ids are the same as the one already cached in memory
            var hashedStoryIds = Utils.Utils.GetSha256Hash(string.Join("", topStoryIds));

            // If already cached return the already processed top stories
            if (cache.TryGetValue(hashedStoryIds, out IEnumerable<StoryDTO> topStories))
            {
                _logger.LogInformation($"<--Returning cached stories for hashed topIds: {hashedStoryIds}");
                return topStories;
            }

            _logger.LogInformation($"---No cache entry found for: {hashedStoryIds}. Getting top story details!");
            topStories = await this.GetStoryDetails(topStoryIds);

            // Save top stories in cache
            cache.Set(hashedStoryIds, topStories.OrderByDescending(s => s.Score), cacheEntryOptions);
            _logger.LogInformation($"-->Cache entry added for: {hashedStoryIds}.");

            return topStories;
        }

        private async Task<IEnumerable<StoryDTO>> GetStoryDetails(IEnumerable<string> topStoryIds)
        {
            List<StoryDTO> topStories = new List<StoryDTO>();

            foreach (var id in topStoryIds)
            {
                // If story already cached add that one to topStories
                if (cache.TryGetValue(id, out StoryDTO cachedStoryDetail))
                {
                    _logger.LogInformation($"<--Cache entry found for: {id}.");
                    topStories.Add(mapper.Map<StoryDTO>(cachedStoryDetail));
                }
                else
                {
                    //Get the story detail from story provider
                    _logger.LogInformation($"---No cache entry found for story: {id}. Getting story detail from service!");

                    Story storyDetail;
                    try
                    {
                        storyDetail = await storyService.GetStoryDetails(id);
                    }
                    catch (Exception ex)
                    {
                        //Log the exception and try to continue to get the other stories
                        _logger.LogError("Error getting best stry ids from story service!", ex);
                        continue;
                    }
                    
                    //Process story detail add to top stories and cache entry
                    if (storyDetail != null)
                    {
                        var mappedStory = mapper.Map<StoryDTO>(storyDetail);
                        topStories.Add(mappedStory);
                        _logger.LogInformation($"-->Cache entry added for: {id}.");
                        cache.Set(id, mappedStory, cacheEntryOptions);
                    }
                }
            }

            // Guarantee that the stories are ordered by score
            return topStories.OrderByDescending(s => s.Score);
        }
    }
}
