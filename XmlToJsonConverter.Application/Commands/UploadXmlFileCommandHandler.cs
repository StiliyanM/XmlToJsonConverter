using MediatR;
using XmlToJsonConverter.Application.Helpers;
using XmlToJsonConverter.Domain.Entities;
using XmlToJsonConverter.Domain.Interfaces;

namespace XmlToJsonConverter.Application.Commands
{
    public class UploadXmlFileCommandHandler : IRequestHandler<UploadXmlFileCommand>
    {
        private readonly IFileConverter _fileConverter;
        private readonly IFileRepository _fileRepository;

        public UploadXmlFileCommandHandler(IFileConverter fileConverter, IFileRepository fileRepository)
        {
            _fileConverter = fileConverter;
            _fileRepository = fileRepository;
        }

        public async Task Handle(UploadXmlFileCommand command, CancellationToken cancellationToken)
        {
            var xmlContent = await FileHelper.ReadFileAsync(command.XmlFile);
            var xmlFile = XmlFile.Build(command.XmlFile.FileName, xmlContent);
            var jsonContent = await _fileConverter.ConvertXmlToJsonAsync(xmlFile);

            await _fileRepository
                .SaveFileAsync(jsonContent, Path.ChangeExtension(command.XmlFile.FileName, ".json"));
        }
    }
}
