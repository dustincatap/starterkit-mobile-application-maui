using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using StarterKit.Maui.Core.Domain.Models;
using StarterKit.Maui.Core.Presentation.ViewModels;
using StarterKit.Maui.Features.Post.Domain.Models;
using StarterKit.Maui.Features.Post.Domain.Services;

namespace StarterKit.Maui.Features.Post.Presentation.ViewModels;

public partial class PostListViewModel : ObservableObject, IInitialize
{
    private readonly ILogger<PostListViewModel> _logger;
    private readonly IPostService _postService;

    public PostListViewModel(ILogger<PostListViewModel> logger,
        IPostService postService)
    {
        _logger = logger;
        _postService = postService;
    }

    [ObservableProperty]
    private PostListState _state = new PostsListLoadingState();

    public async Task OnInitialize(object? parameter = null)
    {
        await GetPosts();
    }

    private async Task GetPosts()
    {
        _logger.LogInformation("Getting posts");

        State = new PostsListLoadingState();

        Result<IEnumerable<PostEntity>> result = await _postService.GetPosts();

        switch (result)
        {
            case Success<IEnumerable<PostEntity>> success:
                State = new PostsListLoadedState(new ObservableCollection<PostEntity>(success.Value));
                _logger.LogInformation("Got {PostCount} posts", success.Value.Count());
                break;

            case Failure<IEnumerable<PostEntity>> failure:
                State = new PostsListErrorState(failure.Exception.Message);
                _logger.LogError(failure.Exception, "Failed to get posts");
                break;
        }
    }
}