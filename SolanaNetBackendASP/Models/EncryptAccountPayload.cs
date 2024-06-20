namespace SolanaNetBackendASP.Models;

[Serializable]
public struct EncryptAccountPayload
{
    public string Password { get; set; }
    public string AccountData { get; set; }
    public string PublicKey { get; set; }
}