using Equatable.Attributes;
using StarterKit.Maui.Core.Domain.Models;

namespace StarterKit.Maui.Features.Post.Domain.Models;

[Equatable]
public partial class PostEntity : IEntity
{
    public required int Id { get; init; }

    [IgnoreEquality]
    public required string Title { get; init; }

    [IgnoreEquality]
    public required string Body { get; init; }
}