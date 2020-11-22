using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HNBestStories.Interfaces;
using HNBestStories.Models;
using Microsoft.AspNetCore.Mvc;

namespace HNBestStories.Controllers
{
    /// <summary>
    /// The stories controller.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StoriesController : ControllerBase
    {
        /// <summary>
        /// The story manager
        /// </summary>
        private readonly IStoryManager storyManager;

        /// <summary>
        /// The stories controller contructor.
        /// </summary>
        /// <param name="storyManager">The story manager.</param>
        public StoriesController(IStoryManager storyManager)
        {
            this.storyManager = storyManager ?? throw new ArgumentNullException(nameof(storyManager));
        }

        /// <summary>
        /// Retieves top best stories.
        /// </summary>
        /// <returns>List of stories.</returns>
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
