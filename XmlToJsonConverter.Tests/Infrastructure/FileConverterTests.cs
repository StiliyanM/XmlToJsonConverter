using Newtonsoft.Json.Linq;
using System.Text;
using System.Xml;
using XmlToJsonConverter.Domain.Entities;
using XmlToJsonConverter.Infrastructure.FileConverters;

namespace XmlToJsonConverter.Tests.Infrastructure;

public class FileConverterTests
{
    [Fact]
    public async Task ConvertXmlToJsonAsync_ReturnsExpectedJson_ForValidXml()
    {
        // Arrange
        var converter = new FileConverter();
        var xmlContent = Encoding.UTF8.GetBytes(
            "<note><to>User</to><from>Service</from><body>Test</body></note>");
        var xmlFile = XmlFile.Build("valid.xml", xmlContent);
        var expectedJson = "{\"note\":{\"to\":\"User\",\"from\":\"Service\",\"body\":\"Test\"}}";

        // Act
        var jsonOutput = await converter.ConvertXmlToJsonAsync(xmlFile, CancellationToken.None);

        // Assert
        var expectedJObject = JObject.Parse(expectedJson);
        var actualJObject = JObject.Parse(jsonOutput);
        Assert.True(JToken.DeepEquals(expectedJObject, actualJObject), 
            "The converted JSON does not match the expected structure.");
    }

    [Fact]
    public async Task ConvertXmlToJsonAsync_ThrowsException_ForInvalidXml()
    {
        // Arrange
        var converter = new FileConverter();
        var invalidXmlContent = Encoding.UTF8.GetBytes("Invalid XML");
        var xmlFile = XmlFile.Build("invalid.xml", invalidXmlContent);

        // Act & Assert
        await Assert.ThrowsAsync<XmlException>(
            () => converter.ConvertXmlToJsonAsync(xmlFile, CancellationToken.None));
    }
}