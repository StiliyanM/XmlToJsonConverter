namespace XmlToJsonConverter.Domain.Interfaces;

public interface IFileRepository
{
    Task SaveFileAsync(string content, string fileName);

}
