using StarterKit.Maui.Core.Infrastructure.Resource;

namespace StarterKit.Maui.Core.Infrastructure.Environment;

public class EnvironmentFileReader
{
	private readonly IEmbeddedResourceReader _embeddedResourceReader;

	public EnvironmentFileReader(IEmbeddedResourceReader embeddedResourceReader)
	{
		_embeddedResourceReader = embeddedResourceReader;
	}

	public IEnvironmentVariables Read(string resourceName, Type typeAssembly)
	{
		EnvironmentVariables? environmentVariables =
			_embeddedResourceReader.ReadAs<EnvironmentVariables>(resourceName, typeAssembly);

		if (environmentVariables == null)
		{
			throw new InvalidOperationException("Could not read environment file");
		}

		return environmentVariables;
	}
}
