using Microsoft.EntityFrameworkCore;
using StarterKit.Maui.Core.Data.Local;
using StarterKit.Maui.Features.Post.Domain.Models;

namespace StarterKit.Maui.Features.Post.Data.Local;

public class PostRepository : BaseRepository<PostEntity>
{
	public PostRepository(DbContext context) : base(context)
	{
	}
}
