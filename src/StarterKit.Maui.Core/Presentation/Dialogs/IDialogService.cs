namespace StarterKit.Maui.Core.Presentation.Dialogs;

public interface IDialogService
{
	Task Alert(string message, string? title = null, string? okText = null);

	Task<bool> Confirm(string message,
		string? title = null,
		string? okText = null,
		string? cancelText = null);
}
