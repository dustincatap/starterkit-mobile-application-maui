using System.Diagnostics.CodeAnalysis;

namespace StarterKit.Maui.Core.Infrastructure.Platform;

[ExcludeFromCodeCoverage]
public class PathProvider : IPathProvider
{
	public string CacheFolderPath => FileSystem.CacheDirectory;

	public string DatabasePath => Path.Combine(CacheFolderPath, "app.db");
}
