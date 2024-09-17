using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace StarterKit.Maui.Features.Post.Domain.Models;

[ExcludeFromCodeCoverage]
public class PostEntityConfiguration : IEntityTypeConfiguration<PostEntity>
{
	public void Configure(EntityTypeBuilder<PostEntity> builder)
	{
		builder.ToTable("Posts");
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Title).IsRequired();
		builder.Property(x => x.Body).IsRequired();
	}
}
