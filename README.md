# HNBestStories

## Overview

The HNBestStories is a RESTful API implemented using ASP.NET 2.2 that retrieves the details of the first 20 best stories from Hacker News.
To run the application download the repository, build and publish the solution and run it with `dotnet HNBestStories.dll` (assuming you publish it as a [framework-dependent](https://docs.microsoft.com/en-us/dotnet/core/deploying/#publish-framework-dependent))

## Assumptions

After reading the Hacker News Api documentation these were the assumptions made:
- The endpoint for beststories retrieves the story ids ordered by score, this way only the top 20 ids need to be used to fetch the story detail;
- The beststories list doesn't change very often so we can use a sliding window of 2 minutes for the in memory cache aproach;

## Enhancements
Given the time I would like to:
- Implement resilience and fail safe patterns using policies such as Retry, Circuit Breaker, Timeout, and Fallback in major points prone to failure, namely Hacker news API communication;
- Abstract the CacheProvider to allow a more flexible aproach to how the API stores the cached data. A good example of this is if the API was to be included in a microservice architectural solution a DestributedCache aproach could be used to guarantee that all instances of the HNBestStories API access the same stored data;
- Add metrics endpoint that could be read by a monitoring and alerting toolkit so a dashboard of some sort could be created to allow real time monitoring of the application;
- After some tests and log analysis refine logging and logging levels;
- Implement UniTests to guarantee the consistency of the application.
