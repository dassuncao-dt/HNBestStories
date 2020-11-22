using HNBestStories.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HNBestStories.Interfaces
{
    public interface IStoryManager
    {
        /// <summary>
        /// Retrieves the best stories.
        /// </summary>
        /// <returns>List of stories.</returns>
        Task<IEnumerable<StoryDTO>> GetBestStories();
    }
}
