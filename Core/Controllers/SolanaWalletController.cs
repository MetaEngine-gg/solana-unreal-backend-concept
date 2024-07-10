using Microsoft.AspNetCore.Mvc;
using SolanaNetBackendASP.Data_Controllers;

namespace SolanaNetBackendASP.Controllers;

[ApiController, Route("api/solana-wallet")]
public class SolanaWalletController : ControllerBase
{
    private readonly UserDataController _userController;

    public SolanaWalletController(UserDataController userController)
    {
        _userController = userController;
    }
    
    [HttpGet, Route("public-key/{username}")]
    public ActionResult<string> GetPublicKey(string username)
    {
        var status = _userController.GetPublicKey(username);
        return status.result ? StatusCode(200, status.text) : StatusCode(404, status.text);
    }
    
    [HttpGet, Route("private-key/{address}")]
    public ActionResult<string> GetPrivateKey(string address)
    {
        var status = _userController.GetPrivateKey(address);
        return status.result ? StatusCode(200, status.text) : StatusCode(404, status.text);
    }

    [HttpGet, Route("balance/{address}")]
    public async Task<ActionResult<ulong>> GetBalance(string address)
    {
        var status = await _userController.GetBalance(address);
        return status.result ? StatusCode(200, status.text) : StatusCode(404, status.text);
    }
}