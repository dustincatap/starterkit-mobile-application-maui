using StarterKit.Maui.Core.Domain.Mappers;
using StarterKit.Maui.Features.Post.Domain.Models;

namespace StarterKit.Maui.Features.Post.Domain.Mappers;

public interface IPostMapper : IObjectMapper<PostDataContract, PostEntity>
{
}
