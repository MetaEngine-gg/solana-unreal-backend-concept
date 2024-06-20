namespace SolanaNetBackendASP.Models.KeyStore;

[Serializable]
public struct RestoreKeyStorePayload
{
    public string PrivateKey { get; set; }
    public string Password { get; set; }
}