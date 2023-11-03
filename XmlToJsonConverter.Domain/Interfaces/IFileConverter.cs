namespace XmlToJsonConverter.Domain.Interfaces
{
    internal interface IFileConverter
    {
        Task<string> ConvertXmlToJsonAsync(File xmlFile);
    }
}
