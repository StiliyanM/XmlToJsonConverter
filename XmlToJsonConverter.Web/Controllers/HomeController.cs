using Microsoft.AspNetCore.Mvc;

namespace XmlToJsonConverter.Web.Controllers;

[Route("")]
[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
