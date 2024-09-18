using Microsoft.Extensions.Logging;
using StarterKit.Maui.Core.Data.Local;
using StarterKit.Maui.Core.Domain.Models;
using StarterKit.Maui.Core.Infrastructure.Platform;
using StarterKit.Maui.Features.Post.Data.Remote;
using StarterKit.Maui.Features.Post.Domain.Mappers;
using StarterKit.Maui.Features.Post.Domain.Models;

namespace StarterKit.Maui.Features.Post.Domain.Services;

public class PostService : IPostService
{
	private readonly ILogger<PostService> _logger;
	private readonly IPostApi _postApi;
	private readonly IRepository<PostEntity> _postRepository;
	private readonly IPostMapper _postMapper;
	private readonly IConnectivityService _connectivityService;

	public PostService(ILogger<PostService> logger,
		IPostApi postApi,
		IRepository<PostEntity> postRepository,
		IPostMapper postMapper,
		IConnectivityService connectivityService)
	{
		_logger = logger;
		_postApi = postApi;
		_postRepository = postRepository;
		_postMapper = postMapper;
		_connectivityService = connectivityService;
	}

	public async Task<Result<IEnumerable<PostEntity>>> GetPosts()
	{
		try
		{
			IEnumerable<PostEntity> savedEntities;

			if (!_connectivityService.IsInternetConnected)
			{
				savedEntities = _postRepository.GetAll();
				return new Success<IEnumerable<PostEntity>>(savedEntities);
			}

			IEnumerable<PostDataContract> contracts = await _postApi.GetPosts();
			IList<PostEntity> entities = contracts.Select(_postMapper.Map).ToList();

			savedEntities = _postRepository.GetAll();
			_postRepository.RemoveAll(savedEntities);
			_postRepository.AddAll(entities);
			await _postRepository.SaveChanges();

			return new Success<IEnumerable<PostEntity>>(entities);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to get posts");
			return new Failure<IEnumerable<PostEntity>>(ex);
		}
	}
}
