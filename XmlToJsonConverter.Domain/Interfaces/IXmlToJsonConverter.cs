namespace XmlToJsonConverter.Domain.Interfaces;

public interface IXmlToJsonConverter
{
    Task<string> ConvertAsync(Stream xmlStream, CancellationToken cancellationToken);
}
