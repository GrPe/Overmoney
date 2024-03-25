using Overmoney.Api.Infrastructure.Converters;
using System.Text.Json.Serialization;

namespace Overmoney.Api.Features.Transactions.Models;

[JsonConverter(typeof(ScheduleJsonConverter))]
public readonly record struct Schedule(string Cron);