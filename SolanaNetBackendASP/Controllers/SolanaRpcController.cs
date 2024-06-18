using Microsoft.AspNetCore.Mvc;
using SolanaNetBackendASP.Data_Controllers;
using SolanaNetBackendASP.Models;

namespace SolanaNetBackendASP.Controllers;

[ApiController, Route("api/solana-rpc")]
public class SolanaRpcController : ControllerBase
{
    private readonly SolnetRpcDataController _solnetMain;

    public SolanaRpcController(SolnetRpcDataController solnetMain)
    {
        _solnetMain = solnetMain;
    }

    [HttpGet, Route("airdrop/{address}")]
    public ActionResult<ulong> RequestAirDrop(string address)
    {
        if (string.IsNullOrEmpty(address))
        {
            return BadRequest("Address is empty");
        }
        
        var isSuccess = _solnetMain.RequestAirDrop(address, 100000);
        return isSuccess ? Ok(isSuccess) : StatusCode(500, $"Air Drop has failed");
    }
    
    [HttpPost, Route("send-transaction")]
    public async Task<ActionResult<bool>> SendTransaction([FromBody] SendTransactionPayload payload)
    {
        if (string.IsNullOrEmpty(payload.FromAddress))
        {
            return BadRequest("From Address is empty");
        }
        
        if (string.IsNullOrEmpty(payload.ToAddress))
        {
            return BadRequest("To Address is empty");
        }
        
        if (payload.FromAddress == payload.ToAddress)
        {
            return BadRequest("From and To addresses are the same");
        }
        
        var isSuccess = await _solnetMain.SendTransaction(payload.FromAddress, payload.ToAddress, payload.Amount);
        return isSuccess ? Ok(isSuccess) : StatusCode(500, $"Transaction has failed");
    }
}