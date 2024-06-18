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
        var result = _userController.GetPublicKey(username);
        return string.IsNullOrEmpty(result) ? Ok(result) : StatusCode(404, $"No public key found for username [{username}]!");
    }
    
    [HttpGet, Route("private-key/{address}")]
    public ActionResult<string> GetPrivateKey(string address)
    {
        var result = _userController.GetPrivateKey(address);
        return string.IsNullOrEmpty(result) ? Ok(result) : StatusCode(404, $"No private key found for address {address}!");
    }

    [HttpGet, Route("balance/{address}")]
    public async Task<ActionResult<ulong>> GetBalance(string address)
    {
        var request = await _userController.GetBalance(address);
        return request.WasSuccessful ? Ok(request.Result.Value) : StatusCode(500, $"Error retrieving balance!");
    }
}