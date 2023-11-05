using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;
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
        var fileMock = CreateFormFileMock("<root>content</root>");
        var converterMock = CreateFileConverterMock();
        var repositoryMock = CreateFileRepositoryMock();
        var validatorMock = CreateValidatorMock();

        var handler = CreateHandler(
            converterMock.Object, repositoryMock.Object, validatorMock.Object);
        var fileAdapter = new FormFileAdapter(fileMock.Object);
        var command = new UploadXmlFileCommand(fileAdapter);

        converterMock.Setup(x => x.ConvertXmlToJsonAsync(It.IsAny<XmlFile>()))
            .ReturnsAsync("valid JSON content");

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        converterMock.Verify(x => x.ConvertXmlToJsonAsync(It.IsAny<XmlFile>()), Times.Once);
        repositoryMock.Verify(
            x => x.SaveFileAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GivenInvalidXml_WhenUploadCommandIsHandled_ErrorIsThrown()
    {
        // Arrange
        var fileMock = CreateFormFileMock("Invalid XML content", "invalid.xml");
        var converterMock = CreateFileConverterMock();
        var repositoryMock = CreateFileRepositoryMock();
        var validatorMock = CreateValidatorMock(isValid: false);

        var handler = CreateHandler(
            converterMock.Object, repositoryMock.Object, validatorMock.Object);
        var fileAdapter = new FormFileAdapter(fileMock.Object);
        var command = new UploadXmlFileCommand(fileAdapter);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => handler.Handle(command, CancellationToken.None));
        repositoryMock.Verify(
            x => x.SaveFileAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GivenValidXml_FileConverterThrowsException_Throws()
    {
        // Arrange
        var fileMock = CreateFormFileMock("<root>content</root>");
        var converterMock = CreateFileConverterMock();
        var repositoryMock = CreateFileRepositoryMock();
        var validatorMock = CreateValidatorMock();

        converterMock.Setup(x => x.ConvertXmlToJsonAsync(It.IsAny<XmlFile>()))
            .ThrowsAsync(new Exception("Conversion failed"));

        var handler = CreateHandler(
            converterMock.Object, repositoryMock.Object, validatorMock.Object);
        var fileAdapter = new FormFileAdapter(fileMock.Object);
        var command = new UploadXmlFileCommand(fileAdapter);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(
            () => handler.Handle(command, CancellationToken.None));
        repositoryMock.Verify(
            x => x.SaveFileAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GivenValidXml_FileRepositoryThrowsException_Throws()
    {
        // Arrange
        var fileMock = CreateFormFileMock("<root>content</root>");
        var converterMock = CreateFileConverterMock();
        var repositoryMock = CreateFileRepositoryMock();
        var validatorMock = CreateValidatorMock();

        converterMock.Setup(x => x.ConvertXmlToJsonAsync(It.IsAny<XmlFile>()))
            .ReturnsAsync("valid JSON content");
        repositoryMock.Setup(x => x.SaveFileAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new IOException("Unable to save file"));

        var handler = CreateHandler(
            converterMock.Object, repositoryMock.Object, validatorMock.Object);
        var fileAdapter = new FormFileAdapter(fileMock.Object);
        var command = new UploadXmlFileCommand(fileAdapter);

        // Act & Assert
        await Assert.ThrowsAsync<IOException>(
            () => handler.Handle(command, CancellationToken.None));
        converterMock.Verify(x => x.ConvertXmlToJsonAsync(It.IsAny<XmlFile>()), Times.Once);
    }

    private static Mock<IFormFile> CreateFormFileMock(
        string content, string fileName = "test.xml")
    {
        var fileMock = new Mock<IFormFile>();
        var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(memoryStream.Length);
        fileMock.Setup(f => f.OpenReadStream()).Returns(memoryStream);
        return fileMock;
    }

    private static Mock<IFileConverter> CreateFileConverterMock()
    {
        var mockConverter = new Mock<IFileConverter>();
        return mockConverter;
    }

    private static Mock<IFileRepository> CreateFileRepositoryMock()
    {
        var mockRepository = new Mock<IFileRepository>();
        return mockRepository;
    }

    private static Mock<IValidator<UploadXmlFileCommand>> CreateValidatorMock(
        bool isValid = true)
    {
        var mockValidator = new Mock<IValidator<UploadXmlFileCommand>>();

        var validationResult = isValid
            ? new ValidationResult()
            : new ValidationResult(new List<ValidationFailure>
                {
                new ValidationFailure("file", "Validation Failed")
                });

        mockValidator.Setup(v => v.ValidateAsync(
                It.IsAny<UploadXmlFileCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        return mockValidator;
    }

    private static UploadXmlFileCommandHandler CreateHandler(
        IFileConverter converter,
        IFileRepository repository,
        IValidator<UploadXmlFileCommand> validator)
    {
        return new UploadXmlFileCommandHandler(converter, repository, validator);
    }
}
