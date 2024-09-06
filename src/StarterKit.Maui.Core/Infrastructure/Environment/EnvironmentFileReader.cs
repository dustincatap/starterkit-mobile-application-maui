using StarterKit.Maui.Core.Infrastructure.Resource;

namespace StarterKit.Maui.Core.Infrastructure.Environment;

public class EnvironmentFileReader
{
    private const string EnvironmentFileName = "configurations.json";

    private readonly IEmbeddedResourceReader _embeddedResourceReader;

    public EnvironmentFileReader(IEmbeddedResourceReader embeddedResourceReader)
    {
        _embeddedResourceReader = embeddedResourceReader;
    }

    public IEnvironmentVariables Read()
    {
        EnvironmentVariables? environmentVariables = _embeddedResourceReader
            .ReadAs<EnvironmentVariables>(EnvironmentFileName, typeof(EnvironmentFileReader));

        if (environmentVariables == null)
        {
            throw new InvalidOperationException("Could not read environment file");
        }

        return environmentVariables;
    }
}