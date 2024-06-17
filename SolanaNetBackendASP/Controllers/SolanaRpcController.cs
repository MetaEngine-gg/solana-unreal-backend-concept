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
        var isSuccess = _solnetMain.RequestAirDrop(address, 100000);
        return isSuccess ? Ok(isSuccess) : StatusCode(500, $"Air Drop has failed");
    }
}