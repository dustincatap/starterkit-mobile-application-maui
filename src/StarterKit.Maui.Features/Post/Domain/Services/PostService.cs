using Microsoft.Extensions.Logging;
using StarterKit.Maui.Core.Data.Local;
using StarterKit.Maui.Core.Domain.Mappers;
using StarterKit.Maui.Core.Domain.Models;
using StarterKit.Maui.Features.Post.Data.Remote;
using StarterKit.Maui.Features.Post.Domain.Models;

namespace StarterKit.Maui.Features.Post.Domain.Services;

public class PostService : IPostService
{
    private readonly ILogger<PostService> _logger;
    private readonly IPostApi _postApi;
    private readonly IRepository<PostEntity> _postRepository;
    private readonly IObjectMapper<PostDataContract, PostEntity> _postMapper;

    public PostService(ILogger<PostService> logger,
        IPostApi postApi,
        IRepository<PostEntity> postRepository,
        IObjectMapper<PostDataContract, PostEntity> postMapper)
    {
        _logger = logger;
        _postApi = postApi;
        _postRepository = postRepository;
        _postMapper = postMapper;
    }

    public async Task<Result<IEnumerable<PostEntity>>> GetPosts()
    {
        try
        {
            IEnumerable<PostDataContract> posts = await _postApi.GetPosts();
            IEnumerable<PostEntity> postEntities = posts.Select(_postMapper.Map).ToList();

            IEnumerable<PostEntity> previousPosts = _postRepository.GetAll();
            _postRepository.RemoveAll(previousPosts);
            _postRepository.AddAll(postEntities);
            _postRepository.SaveChanges();

            return new Success<IEnumerable<PostEntity>>(postEntities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get posts");
            return new Failure<IEnumerable<PostEntity>>(ex);
        }
    }
}