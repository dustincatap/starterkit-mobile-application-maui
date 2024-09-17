using System.Collections.ObjectModel;

namespace StarterKit.Maui.Features.Post.Domain.Models;

public abstract record PostListState;

public sealed record PostsListLoadingState : PostListState;

public sealed record PostsListLoadedState(ObservableCollection<PostEntity> Posts) : PostListState;

public sealed record PostsListErrorState(string ErrorMessage) : PostListState;
