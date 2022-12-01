using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    //private static readonly string[] Summaries = new[]
    //{
    //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    //};

    private readonly IUserManager userManager;

    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger, IUserManager userManager)
    {
        _logger = logger;
        this.userManager = userManager;
    }

    [HttpGet(Name = "get-users")]
    public async Task<ActionResult> Get()
    {
        List<User> users = new List<User>();
        try
        {
            users = await userManager.GetUsers();
            if (users == null || users.Count() < 1)
                return NotFound("No result found!");

        }catch(Exception e)
        {
            _logger.LogError(e.Message);
            throw new Exception("Data access failed. Try again.");
        }
        return Ok(users);
    }
}

