using XmlToJsonConverter.Domain.Entities;

namespace XmlToJsonConverter.Domain.Interfaces
{
    public interface IFileConverter
    {
        Task<string> ConvertXmlToJsonAsync(XmlFile xmlFile, CancellationToken cancellationToken);
    }
}
