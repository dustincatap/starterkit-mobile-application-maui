using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using StarterKit.Maui.Core.Data.Local;
using StarterKit.Maui.Core.Domain.Models;
using StarterKit.Maui.Core.Infrastructure.Platform;
using StarterKit.Maui.Features.Post.Data.Remote;
using StarterKit.Maui.Features.Post.Domain.Mappers;
using StarterKit.Maui.Features.Post.Domain.Models;
using StarterKit.Maui.Features.Post.Domain.Services;

namespace StarterKit.Maui.Features.UnitTests.Post.Domain.Services;

[TestFixture]
public class PostServiceTest
{
	private Mock<ILogger<PostService>> _loggerMock;
	private Mock<IPostApi> _postApiMock;
	private Mock<IRepository<PostEntity>> _postRepositoryMock;
	private Mock<IPostMapper> _postMapperMock;
	private Mock<IConnectivityService> _connectivityServiceMock;

	[SetUp]
	public void Setup()
	{
		_loggerMock = new Mock<ILogger<PostService>>();
		_postApiMock = new Mock<IPostApi>();
		_postRepositoryMock = new Mock<IRepository<PostEntity>>();
		_postMapperMock = new Mock<IPostMapper>();
		_connectivityServiceMock = new Mock<IConnectivityService>();
	}

	private PostService CreateUnitUnderTest()
	{
		return new PostService(_loggerMock.Object,
			_postApiMock.Object,
			_postRepositoryMock.Object,
			new PostMapper(),
			_connectivityServiceMock.Object);
	}

	[Test]
	public async Task GetPosts_ShouldReturnSuccessResultWithPosts_WhenInternetIsConnected()
	{
		PostDataContract postContract = new PostDataContract(1, "Title", "Body");
		PostEntity postEntity = new PostEntity { Id = 1, Title = "Title", Body = "Body" };
		List<PostDataContract> postContracts = [postContract];
		List<PostEntity> postEntities = [postEntity];

		_connectivityServiceMock.SetupGet(x => x.IsInternetConnected).Returns(true);
		_postApiMock.Setup(x => x.GetPosts()).ReturnsAsync(postContracts);
		_postMapperMock.Setup(x => x.Map(postContract)).Returns(postEntity);
		_postRepositoryMock.Setup(x => x.GetAll()).Returns(new List<PostEntity>());

		PostService postService = CreateUnitUnderTest();
		Result<IEnumerable<PostEntity>> result = await postService.GetPosts();

		result.ShouldBeOfType<Success<IEnumerable<PostEntity>>>();
		Success<IEnumerable<PostEntity>>? successResult = result as Success<IEnumerable<PostEntity>>;
		successResult!.Value.ShouldBeEquivalentTo(postEntities);

		_postRepositoryMock.Verify(x => x.RemoveAll(It.IsAny<IEnumerable<PostEntity>>()), Times.Once);
		_postRepositoryMock.Verify(x => x.AddAll(postEntities), Times.Once);
		_postRepositoryMock.Verify(x => x.SaveChanges(), Times.Once);
	}

	[Test]
	public async Task GetPosts_ShouldReturnSuccessResultWithPostsFromCache_WhenInternetIsNotConnected()
	{
		PostEntity postEntity = new PostEntity { Id = 1, Title = "Title", Body = "Body" };
		List<PostEntity> postEntities = [postEntity];

		_connectivityServiceMock.SetupGet(x => x.IsInternetConnected).Returns(false);
		_postRepositoryMock.Setup(x => x.GetAll()).Returns(postEntities);

		PostService postService = CreateUnitUnderTest();
		Result<IEnumerable<PostEntity>> result = await postService.GetPosts();

		result.ShouldBeOfType<Success<IEnumerable<PostEntity>>>();
		Success<IEnumerable<PostEntity>>? successResult = result as Success<IEnumerable<PostEntity>>;
		successResult!.Value.ShouldBeEquivalentTo(postEntities);

		_postRepositoryMock.Verify(x => x.GetAll(), Times.Once);
		_postRepositoryMock.Verify(x => x.RemoveAll(It.IsAny<IEnumerable<PostEntity>>()), Times.Never);
		_postRepositoryMock.Verify(x => x.AddAll(It.IsAny<IEnumerable<PostEntity>>()), Times.Never);
		_postRepositoryMock.Verify(x => x.SaveChanges(), Times.Never);
	}

	[Test]
	public async Task GetPosts_ShouldReturnFailureResult_WhenInternetIsConnectedAndApiThrowsException()
	{
		_connectivityServiceMock.SetupGet(x => x.IsInternetConnected).Returns(true);
		_postApiMock.Setup(x => x.GetPosts()).ThrowsAsync(new Exception());

		PostService postService = CreateUnitUnderTest();
		Result<IEnumerable<PostEntity>> result = await postService.GetPosts();

		result.ShouldBeOfType<Failure<IEnumerable<PostEntity>>>();
		Failure<IEnumerable<PostEntity>>? failureResult = result as Failure<IEnumerable<PostEntity>>;
		failureResult!.Exception.ShouldBeOfType<Exception>();
	}
}
