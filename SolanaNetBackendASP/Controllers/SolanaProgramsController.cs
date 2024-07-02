using Microsoft.AspNetCore.Mvc;
using SolanaNetBackendASP.Data_Controllers;

namespace SolanaNetBackendASP.Controllers;

[ApiController, Route("api/solana-programs")]
public class SolanaProgramsController : ControllerBase
{
    private readonly SolnetProgramsDataController _solnetProgramsDataController;

    public SolanaProgramsController(SolnetProgramsDataController programsDataController)
    {
        _solnetProgramsDataController = programsDataController;
    }

    [HttpPost, Route("run-hello-world-program")]
    public ActionResult<string> EncryptAccountData(string walletAddress)
    {
        if (string.IsNullOrEmpty(walletAddress))
        {
            return StatusCode(400, "Wallet address is required");
        }
    
        var result = _solnetProgramsDataController.RunHelloWorldProgram(walletAddress);
        return result ? StatusCode(200) : StatusCode(500, "Failed to run program");
    }
}