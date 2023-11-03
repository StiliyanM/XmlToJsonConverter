namespace XmlToJsonConverter.Application.Interfaces
{
    public interface IApplicationFile
    {
        string FileName { get; }

        long Length { get; }

        Stream OpenReadStream();
    }
}
