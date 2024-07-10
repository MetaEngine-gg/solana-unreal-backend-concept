using Microsoft.AspNetCore.Mvc;
using SolBridge.Data_Controllers;
using SolBridge.Models.KeyStore;

namespace SolBridge.Controllers;

[ApiController, Route("api/solana-keystore")]
public class SolanaKeystoreController : ControllerBase
{
    private readonly SolnetKeystoreDataController _solnetKeystoreController;

    public SolanaKeystoreController(SolnetKeystoreDataController keystoreController)
    {
        _solnetKeystoreController = keystoreController;
    }

    [HttpPost, Route("encrypt-account-data")]
    public ActionResult<string> EncryptAccountData([FromBody] EncryptAccountPayload payload)
    {
        if (string.IsNullOrEmpty(payload.Password))
        {
            return StatusCode(400, "Password is empty");
        }

        if (string.IsNullOrEmpty(payload.AccountData))
        {
            return StatusCode(400, "Account Data is empty");
        }

        if (string.IsNullOrEmpty(payload.PublicKey))
        {
            return StatusCode(400, "Public Key is empty");
        }

        var status = _solnetKeystoreController.EncryptAccountData(payload);
        return status.result ? StatusCode(200, status.text) : StatusCode(500, status.text);
    }

    [HttpPost, Route("decrypt-account-data")]
    public ActionResult<string> DecryptAccountData([FromBody] DecryptAccountPayload payload)
    {
        if (string.IsNullOrEmpty(payload.Password))
        {
            return StatusCode(400, "Password is empty");
        }

        if (string.IsNullOrEmpty(payload.EncryptedAccountData))
        {
            return StatusCode(400, "Encrypted Account Data is empty");
        }

        var status = _solnetKeystoreController.DecryptAccountData(payload);
        return status.result ? StatusCode(200, status.text) : StatusCode(500, status.text);
    }

    [HttpPost, Route("restore-key-store")]
    public ActionResult<string> RestoreKeyStore([FromBody] RestoreKeyStorePayload payload)
    {
        if (string.IsNullOrEmpty(payload.PrivateKey))
        {
            return StatusCode(400, "Private Key is empty");
        }

        if (string.IsNullOrEmpty(payload.Password))
        {
            return StatusCode(400, "Password is empty");
        }

        var status = _solnetKeystoreController.RestoreKeyStore(payload);
        return status.result ? StatusCode(200, status.text) : StatusCode(500, status.text);
    }
}