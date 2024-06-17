using Microsoft.AspNetCore.Mvc;
using SolanaNetBackendASP.Data_Controllers;

[ApiController, Route("api/solana-wallet")]
public class SolanaWalletController : ControllerBase
{
    private readonly UserDataController _userController;

    public SolanaWalletController(UserDataController userController)
    {
        _userController = userController;
    }

    [HttpGet, Route("balance/{address}")]
    public async Task<ActionResult<ulong>> GetBalance(string address)
    {
        var request = await _userController.GetBalance(address);
        return request.WasSuccessful ? Ok(request.Result.Value) : StatusCode(500, $"Error retrieving balance!");
    }
}