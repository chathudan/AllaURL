using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace AllaURL.API.Controllers;

public class URLControllerTests : TestControllerBase
{ 
    
    [Fact]
    public async Task ForwardUrl_ReturnsBadRequest_WhenTokenIsNullOrWhiteSpace()
    {
        // Act
        var result = await _uRLControllerMock.ForwardUrl(null);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task ForwardUrl_ReturnsRedirectResult_WhenValidTokenIsProvided()
    {
        // Arrange
        var token = "washia";
        var ipAddress = "127.0.0.1";
        var expectedUrl = "https://washia.com.au";
        
        // Act
        var result = await _uRLControllerMock.ForwardUrl(token);

        // Assert
        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.Equal(expectedUrl, redirectResult.Url);
    }
}