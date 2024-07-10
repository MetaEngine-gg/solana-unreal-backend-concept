using Microsoft.AspNetCore.Mvc;
using SolBridge.Data_Controllers;
using SolBridge.Models;

namespace SolBridge.Controllers;

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
    
        var programStatus = _solnetProgramsDataController.RunHelloWorldProgram(walletAddress);
        return programStatus.result ? StatusCode(200, programStatus.text) : StatusCode(500, programStatus.text);
    }
    
    [HttpPost, Route("create-and-send-tokens-to-account")]
    public ActionResult<string> CreateAndSendTokensToAccount([FromBody] CreateAndSendTokensPayload payload)
    {
        if (string.IsNullOrEmpty(payload.InitialAccount) || 
            string.IsNullOrEmpty(payload.OwnerAccount) || string.IsNullOrEmpty(payload.MintAccount))
        {
            return StatusCode(400, "Initial account, owner account, and mint account are required");
        }
    
        var programStatus = _solnetProgramsDataController.CreateAndSendTokensToAccount(payload);
        return programStatus.result ? StatusCode(200, programStatus.text) : StatusCode(500, programStatus.text);
    }
    
    [HttpPost, Route("display-token-balances-of-wallet")]
    public ActionResult<string> DisplayTokenBalancesOfWallet(string walletAddress)
    {
        if (string.IsNullOrEmpty(walletAddress))
        {
            return StatusCode(400, "Wallet address is required");
        }
    
        var payload = _solnetProgramsDataController.DisplayTokenBalancesOfWallet(walletAddress);
        return payload.result ? StatusCode(200, payload.text) : StatusCode(500, payload.text);
    }
}