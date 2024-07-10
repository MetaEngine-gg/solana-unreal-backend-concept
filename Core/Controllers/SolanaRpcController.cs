using Core.Data_Controllers;
using Core.Models.Rpc;
using Microsoft.AspNetCore.Mvc;
using Solnet.Rpc.Models;

namespace Core.Controllers;

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
        return isSuccess ? StatusCode(200, "Air Drop completed successfully") : StatusCode(500, "Air Drop has failed");
    }
    
    [HttpGet, Route("get-account-info/{address}")]
    public async Task<ActionResult<AccountInfo>> GetAccountInfo(string address)
    {
        if (string.IsNullOrEmpty(address))
        {
            return StatusCode(400, "Address is required");
        }
        
        var status = await _solnetRpcController.GetAccountInfo(address);
        return status.result ? StatusCode(200, status.accountInfo) : StatusCode(404, "Account info not found");
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
        
        var status = await _solnetRpcController.SendTransaction(payload.FromAddress, payload.ToAddress, payload.Amount);
        return status.result ? StatusCode(200, "Transaction completed successfully") : StatusCode(500, status.text);
    }
}