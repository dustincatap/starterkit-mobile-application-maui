using Equatable.Attributes;
using SQLite;
using StarterKit.Maui.Core.Domain.Models;

namespace StarterKit.Maui.Features.Post.Domain.Models;

[Equatable]
public partial class PostEntity : IEntity
{
	[PrimaryKey]
	public int Id { get; init; }

	[IgnoreEquality]
	public string? Title { get; set; }

	[IgnoreEquality]
	public string? Body { get; set; }
}
