using HNBestStories.Interfaces;
using HNBestStories.Managers;
using HNBestStories.Services;
using HNBestStories.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HNBestStories.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationComponents(this IServiceCollection services, IConfiguration configuration)
        {
            HNStoryServiceSettings options = new HNStoryServiceSettings();
            configuration.GetSection("HNStoryServiceSettings").Bind(options);

            services.Configure<HNStoryServiceSettings>(configuration.GetSection("HNStoryServiceSettings"));

            services.AddHttpClient<IStoryService, HNStoryService>(c =>
            {
                c.BaseAddress = new Uri(options.ConnectionString);
            });

            services.AddTransient<IStoryManager, StoryManager>();

            return services;
        }
    }
}
