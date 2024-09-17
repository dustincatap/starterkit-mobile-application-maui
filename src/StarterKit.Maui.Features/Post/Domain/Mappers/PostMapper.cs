using Riok.Mapperly.Abstractions;
using StarterKit.Maui.Features.Post.Domain.Models;
using System.Diagnostics.CodeAnalysis;

namespace StarterKit.Maui.Features.Post.Domain.Mappers;

[Mapper]
[ExcludeFromCodeCoverage]
public partial class PostMapper : IPostMapper
{
	public partial PostEntity Map(PostDataContract source);

	public partial PostDataContract Map(PostEntity source);
}
