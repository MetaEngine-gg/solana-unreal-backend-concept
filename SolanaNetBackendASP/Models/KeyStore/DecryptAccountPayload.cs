namespace SolanaNetBackendASP.Models.KeyStore;

[Serializable]
public struct DecryptAccountPayload
{
    public string Password { get; set; }
    public string EncryptedData { get; set; }
}