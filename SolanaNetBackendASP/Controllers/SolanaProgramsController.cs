using Microsoft.AspNetCore.Mvc;
using SolanaNetBackendASP.Data_Controllers;
using SolanaNetBackendASP.Models;

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
    
    [HttpPost, Route("create-and-send-tokens-to-account")]
    public ActionResult<string> CreateAndSendTokensToAccount([FromBody] CreateAndSendTokensPayload payload)
    {
        if (string.IsNullOrEmpty(payload.InitialAccount) || 
            string.IsNullOrEmpty(payload.OwnerAccount) || string.IsNullOrEmpty(payload.MintAccount))
        {
            return StatusCode(400, "Initial account, owner account, and mint account are required");
        }
    
        var result = _solnetProgramsDataController.CreateAndSendTokensToAccount(payload);
        return result ? StatusCode(200) : StatusCode(500, "Failed to run program");
    }
    
    [HttpPost, Route("display-token-balances-of-wallet")]
    public ActionResult<string> DisplayTokenBalancesOfWallet(string walletAddress)
    {
        if (string.IsNullOrEmpty(walletAddress))
        {
            return StatusCode(400, "Wallet address is required");
        }
    
        var result = _solnetProgramsDataController.DisplayTokenBalancesOfWallet(walletAddress);
        return result ? StatusCode(200) : StatusCode(500, "Failed to run program");
    }
}