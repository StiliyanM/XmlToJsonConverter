using MediatR;
using XmlToJsonConverter.Application.Interfaces;

namespace XmlToJsonConverter.Application.Commands
{
    public record UploadXmlFileCommand(IApplicationFile XmlFile) : IRequest { };
}
