using StarterKit.Maui.Core.Domain.Models;

namespace StarterKit.Maui.Features.Post.Domain.Models;

public record PostEntity : IEntity
{
    public required int Id { get; init; }

    public required string Title { get; init; }

    public required string Body { get; init; }
}