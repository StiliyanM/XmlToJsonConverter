using Newtonsoft.Json;
using System.Xml;
using System.Xml.Linq;
using XmlToJsonConverter.Domain.Interfaces;

namespace XmlToJsonConverter.Infrastructure.Converters;

public class XmlToJsonConverterService : IXmlToJsonConverter
{
    public async Task<string> ConvertAsync(Stream xmlStream,
        CancellationToken cancellationToken)
    {
        var settings = new XmlReaderSettings
        {
            Async = true,
            DtdProcessing = DtdProcessing.Prohibit
        };

        using var reader = XmlReader.Create(
            xmlStream, settings);
        var xmlDocument = await XDocument.LoadAsync(
            reader, LoadOptions.None, cancellationToken); // Potential memory issues with large files
        var json = JsonConvert.SerializeXNode(xmlDocument); // Potentially blocking if slow
        return json;
    }
}