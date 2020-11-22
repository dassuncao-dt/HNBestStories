using AutoMapper;
using HNBestStories.Interfaces;
using HNBestStories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HNBestStories.Managers
{
    public class StoryManager : IStoryManager
    {
        /// <summary>
        /// The story manager
        /// </summary>
        private readonly IStoryService storyService;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper mapper;

        public StoryManager(IStoryService storyService, IMapper mapper)
        {
            this.storyService = storyService ?? throw new ArgumentNullException(nameof(storyService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<StoryDTO>> GetBestStories()
        {
            var storyIds = await storyService.GetBestStoriesIds();

            if (storyIds == null || !storyIds.Any())
                return null;

            List<StoryDTO> topStories = new List<StoryDTO>();

            foreach (var id in storyIds.Distinct().Take(20))
            {
                var storyDetail = await storyService.GetStoryDetails(id);

                if (storyDetail != null)
                    topStories.Add(mapper.Map<StoryDTO>(storyDetail));
            }

            return topStories;
        }
    }
}
