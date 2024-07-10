namespace Core.Models.KeyStore;

[Serializable]
public struct RestoreKeyStorePayload
{
    public string PrivateKey { get; set; }
    public string Password { get; set; }
}