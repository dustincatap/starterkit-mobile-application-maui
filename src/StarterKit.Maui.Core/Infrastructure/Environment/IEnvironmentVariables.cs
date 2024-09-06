using Microsoft.Extensions.Logging;

namespace StarterKit.Maui.Core.Infrastructure.Environment;

public interface IEnvironmentVariables
{
    string AppId { get; }

    string AppIdSuffix { get; }

    AppEnvironment AppEnvironment { get; }
    
    LogLevel LogLevel { get; }
    
    bool IsDeveloperFeaturesEnabled { get; }
    
    string ApiBaseUrl { get; }
}