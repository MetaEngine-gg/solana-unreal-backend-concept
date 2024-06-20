namespace SolanaNetBackendASP.Models;

[Serializable]
public struct DecryptAccountPayload
{
    public string Password { get; set; }
    public string EncryptedAccountData { get; set; }
}