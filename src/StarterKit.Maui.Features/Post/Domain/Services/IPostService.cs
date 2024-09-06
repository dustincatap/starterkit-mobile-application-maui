using StarterKit.Maui.Core.Domain.Models;
using StarterKit.Maui.Features.Post.Domain.Models;

namespace StarterKit.Maui.Features.Post.Domain.Services;

public interface IPostService
{
    Task<Result<IEnumerable<PostEntity>>> GetPosts();
}