using FluentValidation;
using XmlToJsonConverter.Application.Commands;

namespace XmlToJsonConverter.Web.Validators;

public class UploadXmlFileCommandValidator : AbstractValidator<UploadXmlFileCommand>
{
    public UploadXmlFileCommandValidator()
    {
        RuleFor(command => command.XmlFile.Length)
            .GreaterThan(0).WithMessage("The file cannot be empty.");

        RuleFor(command => command.XmlFile.FileName)
            .NotEmpty().WithMessage("The file must have a name.")
            .Matches(@"\.xml$").WithMessage("The file must have an .xml extension.");
    }
}
