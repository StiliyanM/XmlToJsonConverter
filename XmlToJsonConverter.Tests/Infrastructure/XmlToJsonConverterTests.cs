using Newtonsoft.Json.Linq;
using System.Text;
using System.Xml;
using XmlToJsonConverter.Infrastructure.Converters;

namespace XmlToJsonConverter.Tests.Infrastructure;

public class XmlToJsonConverterTests
{
    [Fact]
    public async Task ConvertXmlToJsonAsync_ReturnsExpectedJson_ForValidXml()
    {
        // Arrange
        var converter = new XmlToJsonConverterService();
        var xmlString = "<note><to>User</to><from>Service</from><body>Test</body></note>";
        var xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));
        var expectedJson = "{\"note\":{\"to\":\"User\",\"from\":\"Service\",\"body\":\"Test\"}}";

        // Act
        var jsonOutput = await converter.ConvertAsync(
            xmlStream, CancellationToken.None);

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
        var converter = new XmlToJsonConverterService();
        var invalidXmlString = "Invalid XML";
        var invalidXmlStream = new MemoryStream(Encoding.UTF8.GetBytes(invalidXmlString));

        // Act & Assert
        await Assert.ThrowsAsync<XmlException>(
            () => converter.ConvertAsync(invalidXmlStream, CancellationToken.None));
    }
}