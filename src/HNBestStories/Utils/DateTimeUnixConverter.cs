using AutoMapper;
using System;

namespace HNBestStories.Utils
{
    class DateTimeUnixConverter : ITypeConverter<double, DateTime>
    {
        public DateTime Convert(double unixTimeStamp, DateTime destination, ResolutionContext context)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
