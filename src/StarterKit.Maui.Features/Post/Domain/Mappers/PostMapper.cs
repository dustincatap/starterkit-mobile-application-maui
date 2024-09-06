using System.Diagnostics.CodeAnalysis;
using Riok.Mapperly.Abstractions;
using StarterKit.Maui.Core.Domain.Mappers;
using StarterKit.Maui.Features.Post.Domain.Models;

namespace StarterKit.Maui.Features.Post.Domain.Mappers;

[Mapper]
[ExcludeFromCodeCoverage]
public partial class PostMapper : IObjectMapper<PostDataContract, PostEntity>
{
    public partial PostEntity Map(PostDataContract source);
}