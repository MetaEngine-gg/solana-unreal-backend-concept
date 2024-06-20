using System.Text;
using SolanaNetBackendASP.Models;
using Solnet.KeyStore;

namespace SolanaNetBackendASP.Data_Controllers;

public class SolnetKeystoreDataController
{
    public bool EncryptAccountData(EncryptAccountPayload payload)
    {
        var secretKeyStoreService = new SecretKeyStoreService();
        var accountDataBytes = Encoding.UTF8.GetBytes(payload.AccountData);
        // Encrypt a private key, seed or mnemonic associated with a certain address
        var jsonString = secretKeyStoreService.EncryptAndGenerateDefaultKeyStoreAsJson(
            payload.Password,
            accountDataBytes,
            payload.PublicKey
        );
        
        // here you can save encrypted data to a file or database

        return true;
    }
}