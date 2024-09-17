using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;

namespace StarterKit.Maui.Core.Data.Remote;

[ExcludeFromCodeCoverage]
public class HttpLoggingHandler : DelegatingHandler
{
	private readonly string[] _types = ["html", "text", "xml", "json", "txt", "x-www-form-urlencoded"];
	private readonly ILogger<HttpLoggingHandler> _logger;

	public HttpLoggingHandler(ILogger<HttpLoggingHandler> logger)
	{
		_logger = logger;
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		string id = Guid.NewGuid().ToString();
		string msg = $"[{id} - Request]";

		try
		{
			Log($"{msg}========Start==========");
			Log(
				$"{msg} {request.Method} {request.RequestUri?.PathAndQuery} {request.RequestUri?.Scheme}/{request.Version}");
			Log($"{msg} Host: {request.RequestUri?.Scheme}://{request.RequestUri?.Host}");

			foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
			{
				Log($"{msg} {header.Key}: {string.Join(", ", header.Value)}");
			}

			if (request.Content != null)
			{
				foreach (KeyValuePair<string, IEnumerable<string>> header in request.Content.Headers)
				{
					Log($"{msg} {header.Key}: {string.Join(", ", header.Value)}");
				}

				if (request.Content is StringContent || IsTextBasedContentType(request.Headers) ||
					IsTextBasedContentType(request.Content.Headers))
				{
					string result = await request.Content.ReadAsStringAsync(cancellationToken);

					Log($"{msg} Content:");
					Log($"{msg} {result}");
				}
			}

			DateTime start = DateTime.Now;

			HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

			DateTime end = DateTime.Now;

			Log($"{msg} Duration: {end - start}");
			Log($"{msg}==========End==========");

			msg = $"[{id} - Response]";
			Log($"{msg}=========Start=========");

			Log(
				$"{msg} {request.RequestUri?.Scheme.ToUpper()}/{response.Version} {(int)response.StatusCode} {response.ReasonPhrase}");

			foreach (KeyValuePair<string, IEnumerable<string>> header in response.Headers)
			{
				Log($"{msg} {header.Key}: {string.Join(", ", header.Value)}");
			}

			foreach (KeyValuePair<string, IEnumerable<string>> header in response.Content.Headers)
			{
				Log($"{msg} {header.Key}: {string.Join(", ", header.Value)}");
			}

			if (response.Content is StringContent || IsTextBasedContentType(response.Headers) ||
				IsTextBasedContentType(response.Content.Headers))
			{
				start = DateTime.Now;
				string result = await response.Content.ReadAsStringAsync(cancellationToken);
				end = DateTime.Now;

				Log($"{msg} Content:");
				Log($"{msg} {result}");
				Log($"{msg} Duration: {end - start}");
			}

			return response;
		}
		catch (Exception ex)
		{
			Log($"{msg} Failed: {ex.GetType()} {ex.Message}");
			throw;
		}
		finally
		{
			Log($"{msg}==========End==========");
		}
	}

	private bool IsTextBasedContentType(HttpHeaders headers)
	{
		if (!headers.TryGetValues("Content-Type", out IEnumerable<string>? values))
		{
			return false;
		}

		string header = string.Join(" ", values).ToLowerInvariant();

		return _types.Any(t => header.Contains(t));
	}

	private void Log(string message)
	{
		_logger.Log(LogLevel.Information, "{Message}", message);
	}
}
