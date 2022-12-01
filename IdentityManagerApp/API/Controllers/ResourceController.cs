using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ResourceController : ControllerBase
{
    //private static readonly string[] Summaries = new[]
    //{
    //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    //};

    private readonly IResourceManager resourceManager;

    private readonly ILogger<ResourceController> _logger;

    public ResourceController(ILogger<ResourceController> logger, IResourceManager resourceManager)
    {
        _logger = logger;
        this.resourceManager = resourceManager;
    }

    [HttpGet(Name = "get-users")]
    public async Task<ActionResult> Get()
    {
        List<Resource> result = new List<Resource>();
        try
        {
            result = await resourceManager.GetResources();
            if (result == null || result.Count() < 1)
                return NotFound("No result found!");

        }catch(Exception e)
        {
            _logger.LogError(e.Message);
            throw new Exception("Data access failed. Try again.");
        }
        return Ok(result);
    }
}

