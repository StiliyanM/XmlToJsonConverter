using Microsoft.Extensions.Options;
using System.IO.Abstractions;
using XmlToJsonConverter.Domain.Interfaces;

namespace XmlToJsonConverter.Infrastructure.Repositories;

public class FileRepository : IFileRepository
{
    private readonly IFileSystem _fileSystem;
    private readonly IOptionsMonitor<FileSettings> _fileSettingsMonitor;

    public FileRepository(
        IFileSystem fileSystem, IOptionsMonitor<FileSettings> fileSettingsMonitor)
    {
        _fileSystem = fileSystem;
        _fileSettingsMonitor = fileSettingsMonitor;
    }

    public async Task SaveFileAsync(string content, string fileName)
    {
        string outputDirectory = _fileSettingsMonitor.CurrentValue.OutputDirectory;
        var filePath = _fileSystem.Path.Combine(outputDirectory, fileName);

        if (!_fileSystem.Directory.Exists(outputDirectory))
        {
            _fileSystem.Directory.CreateDirectory(outputDirectory);
        }

        using var stream = _fileSystem.FileStream.New(
            filePath, FileMode.Create, FileAccess.Write, FileShare.None);
        using var streamWriter = new StreamWriter(stream);
        await streamWriter.WriteAsync(content);
    }
}
