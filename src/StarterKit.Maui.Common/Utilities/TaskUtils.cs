namespace StarterKit.Maui.Common.Utilities;

public class TaskUtils : ITaskUtils
{
    public async Task Delay(int milliseconds)
    {
        await Task.Delay(milliseconds);
    }
}