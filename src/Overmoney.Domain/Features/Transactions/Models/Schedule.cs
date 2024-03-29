using Overmoney.Domain.Converters;
using System.Text.Json.Serialization;

namespace Overmoney.Domain.Features.Transactions.Models;

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
