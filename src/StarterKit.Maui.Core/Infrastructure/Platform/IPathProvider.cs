namespace StarterKit.Maui.Core.Infrastructure.Platform;

public interface IPathProvider
{
	string CacheFolderPath { get; }
	string DatabasePath { get; }
}
