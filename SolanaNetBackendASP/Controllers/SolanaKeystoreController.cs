using Microsoft.AspNetCore.Mvc;
using SolanaNetBackendASP.Data_Controllers;
using SolanaNetBackendASP.Models;

namespace SolanaNetBackendASP.Controllers;

[ApiController, Route("api/solana-keystore")]
public class SolanaKeystoreController : ControllerBase
{
    private readonly SolnetKeystoreDataController _solnetKeystoreController;

    public SolanaKeystoreController(SolnetKeystoreDataController keystoreController)
    {
        _solnetKeystoreController = keystoreController;
    }
    
    [HttpPost, Route("encrypt-account-data")]
    public ActionResult<bool> EncryptAccountData([FromBody] EncryptAccountPayload payload)
    {
        if (string.IsNullOrEmpty(payload.Password))
        {
            return StatusCode(400, $"Password is empty");
        }
        
        if (string.IsNullOrEmpty(payload.AccountData))
        {
            return StatusCode(400, $"Account Data is empty");
        }
        
        if (string.IsNullOrEmpty(payload.PublicKey))
        {
            return StatusCode(400, $"Public Key is empty");
        }
        
        var isSuccess = _solnetKeystoreController.EncryptAccountData(payload);
        return isSuccess ? Ok(isSuccess) : StatusCode(500, $"Encryption failed");
    }
}