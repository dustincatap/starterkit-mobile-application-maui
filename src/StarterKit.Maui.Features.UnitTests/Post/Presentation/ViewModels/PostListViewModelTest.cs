using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using StarterKit.Maui.Core.Domain.Models;
using StarterKit.Maui.Features.Post.Domain.Models;
using StarterKit.Maui.Features.Post.Domain.Services;
using StarterKit.Maui.Features.Post.Presentation.ViewModels;

namespace StarterKit.Maui.Features.UnitTests.Post.Presentation.ViewModels;

[TestFixture]
public class PostListViewModelTest
{
	private Mock<ILogger<PostListViewModel>> _loggerMock;
	private Mock<IPostService> _postServiceMock;

	[SetUp]
	public void SetUp()
	{
		_loggerMock = new Mock<ILogger<PostListViewModel>>();
		_postServiceMock = new Mock<IPostService>();
	}

	private PostListViewModel CreateUnitUnderTest()
	{
		return new PostListViewModel(_loggerMock.Object, _postServiceMock.Object);
	}

	[Test]
	public async Task OnInitialize_ShouldGetPosts_WhenCalled()
	{
		List<PostEntity> posts =
		[
			new PostEntity { Id = 1, Title = "Title 1", Body = "Body 1" },
			new PostEntity { Id = 2, Title = "Title 2", Body = "Body 2" }
		];

		_postServiceMock.Setup(x => x.GetPosts()).ReturnsAsync(new Success<IEnumerable<PostEntity>>(posts));

		PostListViewModel unitUnderTest = CreateUnitUnderTest();
		await unitUnderTest.OnInitialize();

		unitUnderTest.State.ShouldBeOfType<PostsListLoadedState>();
		PostsListLoadedState state = (PostsListLoadedState)unitUnderTest.State;
		state.Posts.ShouldBe(posts);
	}

	[Test]
	public async Task OnInitialize_ShouldSetErrorState_WhenGetPostsFails()
	{
		_postServiceMock.Setup(x => x.GetPosts())
			.ReturnsAsync(new Failure<IEnumerable<PostEntity>>(new Exception("Error")));

		PostListViewModel unitUnderTest = CreateUnitUnderTest();
		await unitUnderTest.OnInitialize();

		unitUnderTest.State.ShouldBeOfType<PostsListErrorState>();
	}
}
