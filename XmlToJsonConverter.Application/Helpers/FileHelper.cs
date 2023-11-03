using XmlToJsonConverter.Application.Interfaces;

namespace XmlToJsonConverter.Application.Helpers;

public static class FileHelper
{
    public static async Task<byte[]> ReadFileAsync(IApplicationFile file)
    {
        if (file == null)
        {
            throw new ArgumentNullException(nameof(file));
        }

        using var stream = file.OpenReadStream();
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}