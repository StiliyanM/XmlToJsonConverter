using Newtonsoft.Json;
using System.Text;
using System.Xml.Linq;
using XmlToJsonConverter.Domain.Entities;
using XmlToJsonConverter.Domain.Interfaces;

namespace XmlToJsonConverter.Infrastructure.FileConverters;

public class FileConverter : IFileConverter
{
    public async Task<string> ConvertXmlToJsonAsync(XmlFile xmlFile)
    {
        return await Task.Run(() =>
        {
            var xmlDocument = XDocument.Parse(Encoding.UTF8.GetString(xmlFile.Content));

            var json = JsonConvert.SerializeXNode(xmlDocument);
            return json;
        });
    }
}
