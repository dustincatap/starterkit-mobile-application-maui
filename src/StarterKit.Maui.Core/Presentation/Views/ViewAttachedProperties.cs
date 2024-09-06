using System.Diagnostics.CodeAnalysis;

namespace StarterKit.Maui.Core.Presentation.Views;

[ExcludeFromCodeCoverage]
public static class ViewAttachedProperties
{
    private static readonly BindableProperty NameProperty =
        BindableProperty.CreateAttached("Name", typeof(string), typeof(ViewAttachedProperties), default(string));

    public static string GetName(BindableObject view)
    {
        return view.GetValue(NameProperty)?.ToString() ?? string.Empty;
    }

    public static void SetName(BindableObject view, string value)
    {
        view.SetValue(NameProperty, value);
    }
}