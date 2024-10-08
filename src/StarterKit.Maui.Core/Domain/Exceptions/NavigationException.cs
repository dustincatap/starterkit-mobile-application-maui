﻿namespace StarterKit.Maui.Core.Domain.Exceptions;

public class NavigationException : Exception
{
	public NavigationException(string message) : base(message)
	{
	}

	public NavigationException(string message, Exception innerException) : base(message, innerException)
	{
	}
}
