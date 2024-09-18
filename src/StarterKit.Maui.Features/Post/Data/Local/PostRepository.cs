using Microsoft.EntityFrameworkCore;
using StarterKit.Maui.Core.Data.Local;
using StarterKit.Maui.Features.Post.Domain.Models;
using System.Diagnostics.CodeAnalysis;

namespace StarterKit.Maui.Features.Post.Data.Local;

[ExcludeFromCodeCoverage]
public class PostRepository : BaseRepository<PostEntity>
{
	public PostRepository(DbContext context) : base(context)
	{
	}
}
