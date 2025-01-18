using System.Net;
using AllaURL.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace AllaURL.API.Controllers;

public abstract class TestControllerBase
{
    public readonly Mock<HttpContext> _mockHttpContext;
    public readonly Mock<IServiceProvider> _mockServiceProvider;
    public readonly URLController  _uRLControllerMock;
    protected readonly Mock<IDistributedCache> _mockCache;
    protected readonly Mock<AllaUrlDbContext> _mockNFCDbContext;
    
    public TestControllerBase()
    {
        // Mock HttpContext
        _mockHttpContext = new Mock<HttpContext>();

        // Mock the Connection property to return a mocked RemoteIpAddress
        var mockConnection = new Mock<ConnectionInfo>();
        mockConnection.Setup(conn => conn.RemoteIpAddress).Returns(IPAddress.Parse("127.0.0.1"));

        _mockHttpContext.Setup(ctx => ctx.Connection).Returns(mockConnection.Object);
        
        _mockServiceProvider = new Mock<IServiceProvider>();
 
        _mockCache = new Mock<IDistributedCache>();
        _mockNFCDbContext = new Mock<AllaUrlDbContext>();
        
        /*_uRLControllerMock = new URLController(_mockCache.Object, _mockNFCDbContext.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = _mockHttpContext.Object
            }
        };*/
    }
}