using Microsoft.AspNetCore.Mvc;
using SolanaNetBackendASP.Data_Controllers;

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
    
    [HttpGet, Route("send-transaction/{fromAddress}/{toAddress}/{amount}")]
    public async Task<ActionResult<bool>> SendTransaction(string fromAddress, string toAddress, ulong amount)
    {
        if (string.IsNullOrEmpty(fromAddress))
        {
            return BadRequest("From Address is empty");
        }
        
        if (string.IsNullOrEmpty(toAddress))
        {
            return BadRequest("To Address is empty");
        }
        
        if (fromAddress == toAddress)
        {
            return BadRequest("From and To addresses are the same");
        }
        
        var isSuccess = await _solnetMain.SendTransaction(fromAddress, toAddress, amount);
        return isSuccess ? Ok(isSuccess) : StatusCode(500, $"Transaction has failed");
    }
}