namespace XmlToJsonConverter.Domain.Interfaces.Converters;

public interface IXmlToJsonConverter
{
    Task<string> ConvertAsync(Stream xmlFile, CancellationToken cancellationToken);
}
