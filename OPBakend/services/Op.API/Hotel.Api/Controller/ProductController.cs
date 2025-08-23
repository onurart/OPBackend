using Microsoft.AspNetCore.Mvc;
using Op.Presentatiton.Auth;
using OpShared.Security;

namespace Hotel.Api.Controller;
[ApiController]
[Route("api/products")]

public class ProductController: ControllerBase 
{
    [HttpGet(template:"[Action]")]
    [HasPermission(Permissions.Product.Read)]
    public IActionResult Get() => Ok();

    [HttpPost(template:"[Action]")]
    [HasPermission(Permissions.Product.Manage)]
    public IActionResult Create() => Ok();
}