using HNBestStories.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HNBestStories.Interfaces
{
    public interface IStoryManager
    {
        Task<IEnumerable<StoryDTO>> GetBestStories();
    }
}
