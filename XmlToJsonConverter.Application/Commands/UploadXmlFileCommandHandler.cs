﻿using FluentValidation;
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
        private readonly IValidator<UploadXmlFileCommand> _validator;

        public UploadXmlFileCommandHandler(
            IFileConverter fileConverter,
            IFileRepository fileRepository,
            IValidator<UploadXmlFileCommand> validator)
        {
            _fileConverter = fileConverter;
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

            var xmlContent = await FileHelper.ReadFileAsync(command.XmlFile);
            var xmlFile = XmlFile.Build(command.XmlFile.FileName, xmlContent);
            var jsonContent = await _fileConverter.ConvertXmlToJsonAsync(xmlFile, cancellationToken);

            await _fileRepository
                .SaveFileAsync(jsonContent,
                Path.ChangeExtension(command.XmlFile.FileName, ".json"));
        }
    }
}
