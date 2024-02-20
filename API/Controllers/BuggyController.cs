using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController:BaseApiController
{
    private readonly DataContext _context;

    public BuggyController(DataContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet("auth")]
    public ActionResult<string>GetSecret()
    {
        return "secret text";
    }

    [HttpGet("not-found")]
    public ActionResult<AppUser>GetNotFound()
    {
        var thing = _context.Users.Find(-1); //there is not a user with id:-1 (hatayi test etmek icin).
        if(thing == null) return NotFound();
        return thing;
    }

    [HttpGet("server-error")]
    public ActionResult<string>GetServerError()
    {
        var thing = _context.Users.Find(-1); //will return null
        var thingToReturn = thing.ToString(); //we give null ref exception in purpose (null.ToString() is tried).
        return thingToReturn;
    }

    [HttpGet("bad-request")]
    public ActionResult<string>GetBadRequest()
    {
        return BadRequest("This was not a good request!"); //400 code will return.
    }
}
