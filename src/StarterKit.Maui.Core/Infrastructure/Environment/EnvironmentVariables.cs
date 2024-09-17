using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;

namespace StarterKit.Maui.Core.Infrastructure.Environment;

public class EnvironmentVariables : IEnvironmentVariables
{
	public required string AppId { get; init; }

	public required string AppIdSuffix { get; init; }

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public required AppEnvironment AppEnvironment { get; init; }

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public required LogLevel LogLevel { get; init; }

	public required bool IsDeveloperFeaturesEnabled { get; init; }

	public required string ApiBaseUrl { get; init; }
}
