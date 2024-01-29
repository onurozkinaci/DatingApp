using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")] //...../api/users(e.g. https://localhost:5001/api/users)

public class BaseApiController:ControllerBase
{

}
