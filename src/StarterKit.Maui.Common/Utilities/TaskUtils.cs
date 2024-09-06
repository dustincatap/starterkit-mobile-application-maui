using System.Diagnostics.CodeAnalysis;

namespace StarterKit.Maui.Common.Utilities;

[ExcludeFromCodeCoverage]
public class TaskUtils : ITaskUtils
{
    public async Task Delay(int milliseconds)
    {
        await Task.Delay(milliseconds);
    }
}