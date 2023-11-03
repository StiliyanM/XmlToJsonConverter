using MediatR;
using Microsoft.AspNetCore.Mvc;
using XmlToJsonConverter.Application.Commands;
using XmlToJsonConverter.Web.Adapters;

namespace XmlToJsonConverter.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class FileUploadController : ControllerBase
{
    private readonly IMediator _mediator;

    public FileUploadController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var applicationFile = new FormFileAdapter(file);
        var command = new UploadXmlFileCommand(applicationFile);
        await _mediator.Send(command);

        return Ok();
    }
}
