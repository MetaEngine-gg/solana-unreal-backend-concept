namespace SolanaNetBackendASP.Models.KeyStore;

[Serializable]
public struct DecryptAccountPayload
{
    public string Password { get; set; }
    public string EncryptedAccountData { get; set; }
}