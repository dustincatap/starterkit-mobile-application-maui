using Refit;
using StarterKit.Maui.Features.Post.Domain.Models;

namespace StarterKit.Maui.Features.Post.Data.Remote;

public interface IPostApi
{
    [Get("/posts")]
    Task<IEnumerable<PostDataContract>> GetPosts();
}