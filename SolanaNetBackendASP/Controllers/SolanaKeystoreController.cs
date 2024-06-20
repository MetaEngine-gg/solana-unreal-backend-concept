using Microsoft.AspNetCore.Mvc;
using SolanaNetBackendASP.Data_Controllers;
using SolanaNetBackendASP.Models.KeyStore;

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

        var encryptedData = _solnetKeystoreController.EncryptAccountData(payload);
        return !string.IsNullOrEmpty(encryptedData) ? Ok(encryptedData) : StatusCode(500, "Encryption failed");
    }

    [HttpPost, Route("decrypt-account-data")]
    public ActionResult<string> DecryptAccountData([FromBody] DecryptAccountPayload payload)
    {
        if (string.IsNullOrEmpty(payload.Password))
        {
            return StatusCode(400, "Password is empty");
        }

        if (string.IsNullOrEmpty(payload.EncryptedData))
        {
            return StatusCode(400, "Encrypted Data is empty");
        }

        var decryptedData = _solnetKeystoreController.DecryptAccountData(payload);
        return !string.IsNullOrEmpty(decryptedData) ? Ok(decryptedData) : StatusCode(500, "Decryption failed");
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

        var decryptedData = _solnetKeystoreController.RestoreKeyStore(payload);
        return !string.IsNullOrEmpty(decryptedData) ? Ok(decryptedData) : StatusCode(500, "Restore failed");
    }
}