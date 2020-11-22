using HNBestStories.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HNBestStories.Interfaces
{
    public interface IStoryService
    {
        /// <summary>
        /// Retrieves the list of best story ids.
        /// </summary>
        /// <returns>List of ids.</returns>
        Task<IEnumerable<string>> GetBestStoriesIds();

        /// <summary>
        /// Given the story id, retrieves the story detail.
        /// </summary>
        /// <param name="id">The story id.</param>
        /// <returns>Story detail.</returns>
        Task<Story> GetStoryDetails(string id);
    }
}
