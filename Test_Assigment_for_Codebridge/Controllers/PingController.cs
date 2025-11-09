using Microsoft.AspNetCore.Mvc;

namespace Test_Assigment_for_Codebridge.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PingController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public PingController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Get version of the doghouseservice
    /// </summary>
    /// <returns>String with version</returns>
    [HttpGet]
    public async Task<ActionResult> GetVersion()
    {
        return Ok($"Dogshouseservice.Version {_configuration["Version"]}");
    }
}
