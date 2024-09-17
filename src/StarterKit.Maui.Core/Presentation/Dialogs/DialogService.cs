using StarterKit.Maui.Common.Localization;

namespace StarterKit.Maui.Core.Presentation.Dialogs;

public class DialogService : IDialogService
{
	private static Page CurrentPage => Application.Current?.MainPage ??
									   throw new InvalidOperationException("Application.Current.MainPage is null");

	public async Task Alert(string message, string? title = null, string? okText = null)
	{
		await CurrentPage.DisplayAlert(title, message, okText ?? Localization.OkText);
	}

	public async Task<bool> Confirm(string message,
		string? title = null,
		string? okText = null,
		string? cancelText = null)
	{
		return await CurrentPage.DisplayAlert(title, message, okText ?? Localization.OkText,
			cancelText ?? Localization.CancelText);
	}
}
