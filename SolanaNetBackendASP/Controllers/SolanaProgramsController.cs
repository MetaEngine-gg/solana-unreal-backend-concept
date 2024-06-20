using Microsoft.AspNetCore.Mvc;
using SolanaNetBackendASP.Data_Controllers;
using SolanaNetBackendASP.Models.KeyStore;

namespace SolanaNetBackendASP.Controllers;

[ApiController, Route("api/solana-keystore")]
public class SolanaProgramsController : ControllerBase
{
    private readonly SolnetProgramsDataController _solnetProgramsDataController;

    public SolanaProgramsController(SolnetProgramsDataController programsDataController)
    {
        _solnetProgramsDataController = programsDataController;
    }

    // [HttpPost, Route("encrypt-account-data")]
    // public ActionResult<string> EncryptAccountData([FromBody] EncryptAccountPayload payload)
    // {
    //     if (string.IsNullOrEmpty(payload.Password))
    //     {
    //         return StatusCode(400, "Password is empty");
    //     }
    //
    //     if (string.IsNullOrEmpty(payload.AccountData))
    //     {
    //         return StatusCode(400, "Account Data is empty");
    //     }
    //
    //     if (string.IsNullOrEmpty(payload.PublicKey))
    //     {
    //         return StatusCode(400, "Public Key is empty");
    //     }
    //
    //     var encryptedData = _solnetKeystoreController.EncryptAccountData(payload);
    //     return !string.IsNullOrEmpty(encryptedData) ? Ok(encryptedData) : StatusCode(500, "Encryption failed");
    // }
}