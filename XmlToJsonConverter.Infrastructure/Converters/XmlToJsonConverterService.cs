using Newtonsoft.Json;
using System.Xml;
using System.Xml.Linq;
using XmlToJsonConverter.Domain.Interfaces.Converters;

namespace XmlToJsonConverter.Infrastructure.Converters;

public class XmlToJsonConverterService : IXmlToJsonConverter
{
    public async Task<string> ConvertAsync(Stream xmlStream,
        CancellationToken cancellationToken)
    {
        using var reader = XmlReader.Create(
            xmlStream, new XmlReaderSettings { Async = true });
        var xmlDocument = await XDocument.LoadAsync(
            reader, LoadOptions.None, cancellationToken);
        var json = JsonConvert.SerializeXNode(xmlDocument);
        return json;
    }
}