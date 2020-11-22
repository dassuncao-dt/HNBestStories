using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HNBestStories.Interfaces;
using HNBestStories.Models;
using Microsoft.AspNetCore.Mvc;

namespace HNBestStories.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StoriesController : ControllerBase
    {
        /// <summary>
        /// The story manager
        /// </summary>
        private readonly IStoryManager storyManager;

        public StoriesController(IStoryManager storyManager)
        {
            this.storyManager = storyManager ?? throw new ArgumentNullException(nameof(storyManager));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StoryDTO>), 200)]
        public async Task<IActionResult> GetBestStories()
        {
            var bestStories = await storyManager.GetBestStories();

            if (bestStories == null || !bestStories.Any())
                return NotFound("There was no story available.");

            return Ok(bestStories);
        }
    }
}
