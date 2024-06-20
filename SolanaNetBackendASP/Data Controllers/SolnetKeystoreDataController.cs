using System.Text;
using SolanaNetBackendASP.Models;
using Solnet.KeyStore;

namespace SolanaNetBackendASP.Data_Controllers;

public class SolnetKeystoreDataController
{
    private readonly ILogger<SolnetKeystoreDataController> _logger;
    private readonly SecretKeyStoreService _secretKeyStoreService;

    public SolnetKeystoreDataController(ILogger<SolnetKeystoreDataController> logger)
    {
        _logger = logger;
        _secretKeyStoreService = new SecretKeyStoreService();
    }

    public string EncryptAccountData(EncryptAccountPayload payload)
    {
        var accountDataBytes = Encoding.UTF8.GetBytes(payload.AccountData);
        // Encrypt a private key, seed or mnemonic associated with a certain address
        var jsonString = _secretKeyStoreService.EncryptAndGenerateDefaultKeyStoreAsJson(
            payload.Password,
            accountDataBytes,
            payload.PublicKey
        );

        // here you can save encrypted data to a file or database

        return jsonString;
    }

    public string DecryptAccountData(DecryptAccountPayload payload)
    {
        // Or decrypt a web3 secret storage encrypted json data
        byte[] data = [];
        try
        {
            data = _secretKeyStoreService.DecryptKeyStoreFromJson(payload.Password, payload.EncryptedAccountData);
        }
        catch (Exception)
        {
            _logger.LogError("Failed to decrypt account data, invalid password or encrypted data");
        }
        
        return Encoding.UTF8.GetString(data);
    }
}