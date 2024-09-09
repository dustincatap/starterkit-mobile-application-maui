using Microsoft.Extensions.Logging;
using Moq;
using StarterKit.Maui.Common.Constants;
using StarterKit.Maui.Common.Utilities;
using StarterKit.Maui.Core.Presentation.Navigation;
using StarterKit.Maui.Features.Startup.Presentation.ViewModels;

namespace StarterKit.Maui.Features.UnitTests.Startup.Presentation.ViewModels;

[TestFixture]
public class SplashViewModelTest
{
    private Mock<ILogger<SplashViewModel>> _loggerMock;
    private Mock<INavigationService> _navigationServiceMock;
    private Mock<ITaskUtils> _taskUtilsMock;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<SplashViewModel>>();
        _navigationServiceMock = new Mock<INavigationService>();
        _taskUtilsMock = new Mock<ITaskUtils>();
    }

    private SplashViewModel CreateUnitUnderTest()
    {
        return new SplashViewModel(_loggerMock.Object,
            _navigationServiceMock.Object,
            _taskUtilsMock.Object);
    }

    [Test]
    public async Task OnInitialize_ShouldNavigateToPostList_WhenCalled()
    {
        SplashViewModel unitUnderTest = CreateUnitUnderTest();

        await unitUnderTest.OnInitialize();

        _navigationServiceMock.Verify(x => x.PushToNewRoot(ViewNames.PostList), Times.Once);
    }
}