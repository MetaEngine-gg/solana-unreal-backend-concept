using Microsoft.AspNetCore.Mvc;
using SolanaNetBackendASP.Data_Controllers;
using SolanaNetBackendASP.Models.Rpc;
using Solnet.Rpc.Models;

namespace SolanaNetBackendASP.Controllers;

[ApiController, Route("api/solana-rpc")]
public class SolanaRpcController : ControllerBase
{
    private readonly SolnetRpcDataController _solnetRpcController;

    public SolanaRpcController(SolnetRpcDataController solnetMain)
    {
        _solnetRpcController = solnetMain;
    }

    [HttpGet, Route("airdrop/{address}")]
    public ActionResult<ulong> RequestAirDrop(string address)
    {
        if (string.IsNullOrEmpty(address))
        {
            return BadRequest("Address is empty");
        }
        
        var isSuccess = _solnetRpcController.RequestAirDrop(address, 100000);
        return isSuccess ? Ok(isSuccess) : StatusCode(500, $"Air Drop has failed");
    }
    
    [HttpGet, Route("get-account-info/{address}")]
    public async Task<ActionResult<AccountInfo>> GetAccountInfo(string address)
    {
        if (string.IsNullOrEmpty(address))
        {
            return StatusCode(400, "Address is required");
        }
        
        var (result, info) = await _solnetRpcController.GetAccountInfo(address);
        return result ? Ok(info) : StatusCode(404, "Account info not found");
    }
    
    [HttpPost, Route("send-transaction")]
    public async Task<ActionResult<bool>> SendTransaction([FromBody] SendTransactionPayload payload)
    {
        if (string.IsNullOrEmpty(payload.FromAddress))
        {
            return StatusCode(400, $"From Address is empty");
        }
        
        if (string.IsNullOrEmpty(payload.ToAddress))
        {
            return StatusCode(400, $"To Address is empty");
        }
        
        if (payload.FromAddress == payload.ToAddress)
        {
            return StatusCode(400, $"From and To Address cannot be the same");
        }
        
        var isSuccess = await _solnetRpcController.SendTransaction(payload.FromAddress, payload.ToAddress, payload.Amount);
        return isSuccess ? Ok(isSuccess) : StatusCode(500, $"Transaction has failed");
    }
}