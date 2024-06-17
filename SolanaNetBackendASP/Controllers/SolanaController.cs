using Microsoft.AspNetCore.Mvc;
using SolanaNetBackendASP.Data_Controllers;

[ApiController, Route("api/[controller]")]
public class SolanaController : ControllerBase
{
    private readonly ILogger<SolanaController> _logger;
    private readonly SolanaRpc _solanaMain;
    private readonly UserDataController _userController;

    public SolanaController(ILogger<SolanaController> logger, SolanaRpc solanaMain, UserDataController userController)
    {
        _logger = logger;
        _solanaMain = solanaMain;
        _userController = userController;
    }

    [HttpGet, Route("balance/{address}")]
    public async Task<ActionResult<ulong>> GetBalance(string address)
    {
        var request = await _userController.GetBalance(address);
        if (request.WasSuccessful)
        {
            return Ok(request.Result.Value);
        }
        else
        {
            return StatusCode(500, $"Error retrieving balance!");
        }
    }

    [HttpGet, Route("airdrop/{address}")]
    public ActionResult<ulong> RequestAirDrop(string address)
    {
        var isSuccess = _solanaMain.RequestAirDrop(address, 100000);
        if (isSuccess)
        {
            return Ok(isSuccess);
        }
        else
        {
            return StatusCode(500, $"Air Drop has failed");
        }
    }
}