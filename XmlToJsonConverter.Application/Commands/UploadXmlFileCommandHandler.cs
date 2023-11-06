using FluentValidation;
using MediatR;
using XmlToJsonConverter.Domain.Interfaces;
using XmlToJsonConverter.Domain.Interfaces.Converters;

namespace XmlToJsonConverter.Application.Commands
{
    public class UploadXmlFileCommandHandler : IRequestHandler<UploadXmlFileCommand>
    {
        private readonly IXmlToJsonConverter _xmlToJsonConverter;
        private readonly IFileRepository _fileRepository;
        private readonly IValidator<UploadXmlFileCommand> _validator;

        public UploadXmlFileCommandHandler(
            IXmlToJsonConverter xmlToJsonConverter,
            IFileRepository fileRepository,
            IValidator<UploadXmlFileCommand> validator)
        {
            _xmlToJsonConverter = xmlToJsonConverter;
            _fileRepository = fileRepository;
            _validator = validator;
        }

        public async Task Handle(
            UploadXmlFileCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var jsonContent = await _xmlToJsonConverter.ConvertAsync(
                command.XmlFile.OpenReadStream(), cancellationToken);

            await _fileRepository
                .SaveFileAsync(jsonContent,
                Path.ChangeExtension(command.XmlFile.FileName, ".json"));
        }
    }
}
