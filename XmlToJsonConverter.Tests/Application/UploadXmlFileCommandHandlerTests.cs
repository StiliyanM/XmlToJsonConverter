using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;
using System.Xml;
using XmlToJsonConverter.Application.Commands;
using XmlToJsonConverter.Domain.Entities;
using XmlToJsonConverter.Domain.Interfaces;
using XmlToJsonConverter.Web.Adapters;

namespace XmlToJsonConverter.Tests.Application;

public class UploadXmlFileCommandHandlerTests
{
    [Fact]
    public async Task GivenValidXml_WhenUploadCommandIsHandled_FileIsConvertedAndSaved()
    {
        // Arrange
        var mockConverter = new Mock<IFileConverter>();
        var mockRepository = new Mock<IFileRepository>();
        var handler = new UploadXmlFileCommandHandler(mockConverter.Object, mockRepository.Object);

        var fileMock = new Mock<IFormFile>();
        var fileName = "test.xml";
        var content = new MemoryStream(Encoding.UTF8.GetBytes("<root>content</root>"));
        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(content.Length);
        fileMock.Setup(f => f.OpenReadStream()).Returns(content);

        var fileAdapter = new FormFileAdapter(fileMock.Object);
        var command = new UploadXmlFileCommand(fileAdapter);

        mockConverter.Setup(x => x.ConvertXmlToJsonAsync(It.IsAny<XmlFile>()))
            .ReturnsAsync("valid JSON content");

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        mockConverter.Verify(x => x.ConvertXmlToJsonAsync(It.IsAny<XmlFile>()), Times.Once);
        mockRepository.Verify(x => x.SaveFileAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GivenInvalidXml_WhenUploadCommandIsHandled_ErrorIsThrown()
    {
        // Arrange
        var mockConverter = new Mock<IFileConverter>();
        var mockRepository = new Mock<IFileRepository>();
        var handler = new UploadXmlFileCommandHandler(mockConverter.Object, mockRepository.Object);

        var fileMock = new Mock<IFormFile>();
        var fileName = "invalid.xml";
        var invalidXmlContent = Encoding.UTF8.GetBytes("Invalid XML content");
        var stream = new MemoryStream(invalidXmlContent);

        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(stream.Length);
        fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
        var fileAdapter = new FormFileAdapter(fileMock.Object);

        var command = new UploadXmlFileCommand(fileAdapter);

        mockConverter.Setup(x => x.ConvertXmlToJsonAsync(It.IsAny<XmlFile>()))
            .ThrowsAsync(new XmlException("Invalid XML"));

        // Act & Assert
        await Assert.ThrowsAsync<XmlException>(() => handler.Handle(command, CancellationToken.None));
        mockRepository.Verify(x => x.SaveFileAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }


    [Fact]
    public async Task GivenValidXml_FileConverterThrowsException_Throws()
    {
        // Arrange
        var mockConverter = new Mock<IFileConverter>();
        var mockRepository = new Mock<IFileRepository>();
        var handler = new UploadXmlFileCommandHandler(mockConverter.Object, mockRepository.Object);

        var fileMock = new Mock<IFormFile>();
        var fileName = "test.xml";
        var validXmlContent = Encoding.UTF8.GetBytes("<root>content</root>");
        var stream = new MemoryStream(validXmlContent);

        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(stream.Length);
        fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
        var fileAdapter = new FormFileAdapter(fileMock.Object);

        var command = new UploadXmlFileCommand(fileAdapter);

        mockConverter.Setup(x => x.ConvertXmlToJsonAsync(It.IsAny<XmlFile>()))
            .ThrowsAsync(new Exception("Conversion failed"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        mockRepository.Verify(x => x.SaveFileAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GivenValidXml_FileRepositoryThrowsException_Throws()
    {
        // Arrange
        var mockConverter = new Mock<IFileConverter>();
        var mockRepository = new Mock<IFileRepository>();
        var handler = new UploadXmlFileCommandHandler(mockConverter.Object, mockRepository.Object);

        var fileMock = new Mock<IFormFile>();
        var fileName = "test.xml";
        var validXmlContent = Encoding.UTF8.GetBytes("<root>content</root>");
        var stream = new MemoryStream(validXmlContent);

        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(stream.Length);
        fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
        var fileAdapter = new FormFileAdapter(fileMock.Object);

        var command = new UploadXmlFileCommand(fileAdapter);

        mockConverter.Setup(x => x.ConvertXmlToJsonAsync(It.IsAny<XmlFile>()))
            .ReturnsAsync("valid JSON content");
        mockRepository.Setup(x => x.SaveFileAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new IOException("Unable to save file"));

        // Act & Assert
        await Assert.ThrowsAsync<IOException>(() => handler.Handle(command, CancellationToken.None));
        mockConverter.Verify(x => x.ConvertXmlToJsonAsync(It.IsAny<XmlFile>()), Times.Once);
    }

    [Fact]
    public async Task GivenNullOrEmptyFile_ThrowsArgumentException()
    {
        // Arrange
        var mockConverter = new Mock<IFileConverter>();
        var mockRepository = new Mock<IFileRepository>();
        var handler = new UploadXmlFileCommandHandler(mockConverter.Object, mockRepository.Object);

        // Act & Assert for null file
        var nullException = await Record.ExceptionAsync(() => handler.Handle(null, CancellationToken.None));
        Assert.IsType<ArgumentException>(nullException);

        // Prepare an empty file using the FormFileAdapter
        var fileMock = new Mock<IFormFile>();
        var emptyStream = new MemoryStream(Array.Empty<byte>());
        fileMock.Setup(f => f.FileName).Returns("empty.xml");
        fileMock.Setup(f => f.Length).Returns(emptyStream.Length);
        fileMock.Setup(f => f.OpenReadStream()).Returns(emptyStream);
        var fileAdapter = new FormFileAdapter(fileMock.Object);

        // Create the command with the empty file
        var emptyCommand = new UploadXmlFileCommand(fileAdapter);

        // Act & Assert for empty file
        var emptyException = await Record.ExceptionAsync(() => handler.Handle(emptyCommand, CancellationToken.None));
        Assert.IsType<ArgumentException>(emptyException);
    }
}
