using System;
using System.Collections.Generic;
using System.Text;

namespace HNBestStories.Settings
{
    public class HNStoryServiceSettings
    {
        public string ConnectionString { get; set; } = "https://hacker-news.firebaseio.com/";
        public string GetStoryIdsResource { get; set; } = "/v0/beststories.json";
        public string GetStoryDetailsResource { get; set; } = "/v0/item/";
    }
}
