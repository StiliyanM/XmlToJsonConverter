using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using XmlToJsonConverter.Application.Commands;
using XmlToJsonConverter.Web.Controllers;

namespace XmlToJsonConverter.Tests.Web;

public class FileUploadControllerTests
{
    [Fact]
    public async Task Upload_InvokesMediatorWithCorrectCommand()
    {
        // Arrange
        var mockMediator = new Mock<IMediator>();
        var controller = new FileUploadController(mockMediator.Object);

        var mockFormFile = new Mock<IFormFile>();
        mockFormFile.Setup(f => f.FileName).Returns("test.xml");
        mockFormFile.Setup(f => f.Length).Returns(1024);

        // Act
        var result = await controller.Upload(mockFormFile.Object);

        // Assert
        mockMediator.Verify(m => m.Send(It.IsAny<UploadXmlFileCommand>(), It.IsAny<CancellationToken>()), Times.Once);

        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }
}
