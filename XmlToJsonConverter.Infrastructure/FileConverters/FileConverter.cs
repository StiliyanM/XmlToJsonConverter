using Newtonsoft.Json;
using System.Xml;
using System.Xml.Linq;
using XmlToJsonConverter.Domain.Entities;
using XmlToJsonConverter.Domain.Interfaces;

namespace XmlToJsonConverter.Infrastructure.FileConverters;

public class FileConverter : IFileConverter
{
    public async Task<string> ConvertXmlToJsonAsync(XmlFile xmlFile,
        CancellationToken cancellationToken)
    {
        using var reader = XmlReader.Create(
            new MemoryStream(xmlFile.Content), new XmlReaderSettings { Async = true });
        var xmlDocument = await XDocument.LoadAsync(
            reader, LoadOptions.None, cancellationToken);
        var json = JsonConvert.SerializeXNode(xmlDocument);
        return json;
    }
}