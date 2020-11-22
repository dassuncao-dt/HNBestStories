using AutoMapper.Configuration.Conventions;
using System;
using System.Collections.Generic;
using System.Text;

namespace HNBestStories.Models
{
    public class Story
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [MapTo(nameof(StoryDTO.Uri))]
        public string Url { get; set; }

        [MapTo(nameof(StoryDTO.PostedBy))]
        public string By { get; set; }

        public double Time { get; set; }

        public int Score { get; set; }

        [MapTo(nameof(StoryDTO.CommentCount))]
        public int Descendants { get; set; }

        public int[] Kids { get; set; }

        public string Type { get; set; }
    }
}
