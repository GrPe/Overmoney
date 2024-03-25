using Overmoney.Api.Infrastructure.Converters;
using System.Text.Json.Serialization;

namespace Overmoney.Api.Features.Transactions.Models;

[JsonConverter(typeof(ScheduleJsonConverter))]
public readonly record struct Schedule(string Cron)
{    
    internal DateTime NextOccurrence(DateTime currentDate)
    {
        return Cronos
            .CronExpression
            .Parse(Cron)
            .GetNextOccurrence(currentDate)
            !.Value;
    }
}
