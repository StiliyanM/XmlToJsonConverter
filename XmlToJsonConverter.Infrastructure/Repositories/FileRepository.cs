using XmlToJsonConverter.Domain.Interfaces;

namespace XmlToJsonConverter.Infrastructure.Repositories;

public class FileRepository : IFileRepository
{
    private readonly string _outputDirectory;

    public FileRepository(string outputDirectory)
    {
        _outputDirectory = outputDirectory;

        if (!Directory.Exists(_outputDirectory))
        {
            Directory.CreateDirectory(_outputDirectory);
        }
    }

    public async Task SaveFileAsync(string content, string fileName)
    {
        var filePath = Path.Combine(_outputDirectory, fileName);
        await File.WriteAllTextAsync(filePath, content);
    }
}
