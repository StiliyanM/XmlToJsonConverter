using XmlToJsonConverter.Domain.Exceptions;

namespace XmlToJsonConverter.Domain.Entities;

public class XmlFile
{
    public string Name { get; private set; }

    public byte[] Content { get; private set; }

    public long Size => Content?.Length ?? 0;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private XmlFile() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public static XmlFile Build(string name, byte[] content)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new FileException("XML file name must be provided.");
        }
        if (content == null || content.Length == 0)
        {
            throw new FileException("XML file content cannot be empty.");
        }

        var xmlFile = new XmlFile
        {
            Name = name,
            Content = content
        };
        return xmlFile;
    }
}
