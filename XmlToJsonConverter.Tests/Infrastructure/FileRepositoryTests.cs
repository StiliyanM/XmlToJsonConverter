using Microsoft.Extensions.Options;
using Moq;
using System.IO.Abstractions.TestingHelpers;
using XmlToJsonConverter.Infrastructure;
using XmlToJsonConverter.Infrastructure.Repositories;

namespace XmlToJsonConverter.Tests.Infrastructure;

public class FileRepositoryTests
{
    [Fact]
    public async Task SaveFileAsync_WritesContentToFile()
    {
        // Arrange
        var mockFileSystem = CreateMockFileSystem();

        var outputDirectory = @"c:\output";

        mockFileSystem.Directory.CreateDirectory(outputDirectory);

        var mockOptionsMonitor = CreateOptionsMonitorMock(outputDirectory);
        var fileRepository = new FileRepository(mockFileSystem, mockOptionsMonitor.Object);

        var fileName = "test.json";
        var fileContent = "Test content";

        // Act
        await fileRepository.SaveFileAsync(fileContent, fileName);

        // Assert
        var filePath = mockFileSystem.Path.Combine(outputDirectory, fileName);

        Assert.True(mockFileSystem.File.Exists(filePath), 
            "The file does not exist in the mock file system.");
        var writtenContent = mockFileSystem.File.ReadAllText(filePath);
        Assert.Equal(fileContent, writtenContent);
    }

    [Fact]
    public async Task SaveFileAsync_CreatesDirectoryIfNotExists()
    {
        // Arrange
        var mockFileSystem = CreateMockFileSystem();
        var outputDirectory = @"c:\output";
        var fileName = "test.json";
        var fileContent = "content";

        var mockOptionsMonitor = CreateOptionsMonitorMock(outputDirectory);
        var fileRepository = new FileRepository(mockFileSystem, mockOptionsMonitor.Object);

        // Act
        await fileRepository.SaveFileAsync(fileContent, fileName);

        // Assert
        Assert.True(mockFileSystem.Directory.Exists(outputDirectory));
    }

    [Fact]
    public async Task SaveFileAsync_OverwritesExistingFile()
    {
        // Arrange
        var mockFileSystem = CreateMockFileSystem();
        var outputDirectory = @"c:\output";
        var fileName = "test.json";
        var initialContent = "initial content";
        var newContent = "new content";

        mockFileSystem.Directory.CreateDirectory(outputDirectory);
        var filePath = mockFileSystem.Path.Combine(outputDirectory, fileName);
        mockFileSystem.File.WriteAllText(filePath, initialContent);

        var mockOptionsMonitor = CreateOptionsMonitorMock(outputDirectory);
        var fileRepository = new FileRepository(mockFileSystem, mockOptionsMonitor.Object);

        // Act
        await fileRepository.SaveFileAsync(newContent, fileName);

        // Assert
        var writtenContent = mockFileSystem.File.ReadAllText(filePath);
        Assert.Equal(newContent, writtenContent);
    }

    [Fact]
    public async Task SaveFileAsync_AllowsOnlyOneWriteAtATime()
    {
        // Arrange
        var mockFileSystem = CreateMockFileSystem();
        var outputDirectory = mockFileSystem.Path.GetTempPath();
        var fileName = "concurrent_test.json";
        var content = "Test content";

        var mockOptionsMonitor = CreateOptionsMonitorMock(outputDirectory);
        var fileRepository = new FileRepository(mockFileSystem, mockOptionsMonitor.Object);

        var filePath = mockFileSystem.Path.Combine(outputDirectory, fileName);

        // Act
        var tasks = Enumerable.Range(0, 10).Select(
            _ => fileRepository.SaveFileAsync(content, fileName));
        await Task.WhenAll(tasks);

        // Assert
        var writtenContent = mockFileSystem.File.ReadAllText(filePath);
        Assert.Equal(content, writtenContent);
        Assert.Single(mockFileSystem.AllFiles);
    }

    [Fact]
    public async Task SaveFileAsync_OverwritesExistingFileWithNewContent()
    {
        // Arrange
        var mockFileSystem = CreateMockFileSystem();
        var outputDirectory = @"c:\output";
        var fileName = "test.json";
        var initialContent = "initial content";
        var newContent = "new content";
        var filePath = mockFileSystem.Path.Combine(outputDirectory, fileName);

        mockFileSystem.AddFile(filePath, new MockFileData(initialContent));

        var mockOptionsMonitor = CreateOptionsMonitorMock(outputDirectory);
        var fileRepository = new FileRepository(mockFileSystem, mockOptionsMonitor.Object);

        // Act
        await fileRepository.SaveFileAsync(newContent, fileName);

        // Assert
        var writtenContent = mockFileSystem.File.ReadAllText(filePath);
        Assert.Equal(newContent, writtenContent);
    }

    private static MockFileSystem CreateMockFileSystem()
        => new();


    private static Mock<IOptionsMonitor<FileSettings>> CreateOptionsMonitorMock(string outputDirectory)
    {
        var mockOptionsMonitor = new Mock<IOptionsMonitor<FileSettings>>();
        mockOptionsMonitor
            .Setup(o => o.CurrentValue).Returns(new FileSettings { OutputDirectory = outputDirectory });
        return mockOptionsMonitor;
    }
}
