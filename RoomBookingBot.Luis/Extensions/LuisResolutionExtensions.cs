﻿using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoomBookingBot.Luis.Extensions
{
    public static class LuisResolutionExtensions
    {
        public static DateTime ProcessDateTimeV2Date(this EntityModel entity)
        {
            if (entity.AdditionalProperties.TryGetValue("resolution", out dynamic resolution))
            {
                var resolutionValues = (IEnumerable<dynamic>)resolution.values;
                var datetimes = resolutionValues.Select(val => DateTime.Parse(val.value.Value));

                if (datetimes.Count() > 1)
                {
                    // assume the date is in the next 7 days
                    var bestGuess = datetimes.Single(dateTime => dateTime > DateTime.Now && (DateTime.Now - dateTime).Days <= 7);
                    return bestGuess;
                }

                return datetimes.FirstOrDefault();
            }

            throw new Exception("ProcessDateTimeV2DateTime");
        }

        public static string ProcessRoom(this EntityModel entity)
        {
            if (entity.AdditionalProperties.TryGetValue("resolution", out dynamic resolution))
            {
                var resolutionValues = (IEnumerable<dynamic>)resolution.values;
                return resolutionValues.Select(room => room).FirstOrDefault();
            }

            throw new Exception("ProcessRoom");
        }

        public static DateTime ProcessDateTimeV2DateTime(this EntityModel entity)
        {
            if (entity.AdditionalProperties.TryGetValue("resolution", out dynamic resolution))
            {
                var resolutionValues = (IEnumerable<dynamic>)resolution.values;
                var datetimes = resolutionValues.Select(val => DateTime.Parse(val.value.Value));

                if (datetimes.Count() > 1)
                {
                    // assume the date is in the next 7 days and falls between 7 AM - 7 PM is the most appropriate date 
                    var bestGuess = datetimes.Single(dateTime => dateTime.Hour > 7 && dateTime.Hour <= 19 && dateTime > DateTime.Now && (DateTime.Now - dateTime).Days <= 7);
                    return bestGuess;
                }

                return datetimes.FirstOrDefault();
            }

            throw new Exception("ProcessDateTimeV2DateTime");
        }

        public static (DateTime start, DateTime end) ProcessDateTimeV2DateTimeRange(this EntityModel entity)
        {
            if (entity.AdditionalProperties.TryGetValue("resolution", out dynamic resolution))
            {
                var resolutionValues = (IEnumerable<dynamic>)resolution.values;
                var datetimeranges = resolutionValues.Select(val => new { start = DateTime.Parse(val.start.Value), end = DateTime.Parse(val.end.Value) });

                if (datetimeranges.Count() > 1)
                {
                    // assume the date is in the next 7 days and falls between 7 AM - 7 PM is the most appropriate date 
                    var bestGuess = datetimeranges.Single(dateTimeRange => dateTimeRange.start.Hour > 7 && dateTimeRange.start.Hour <= 19 && dateTimeRange.start > DateTime.Now && (DateTime.Now - dateTimeRange.start).Days <= 7);
                    return (bestGuess.start, bestGuess.end);
                }

                return (datetimeranges.FirstOrDefault().start, datetimeranges.FirstOrDefault().end);
            }

            throw new Exception("ProcessDateTimeV2DateTimeRange");
        }
    }
}
