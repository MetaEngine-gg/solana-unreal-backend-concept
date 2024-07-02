namespace SolanaNetBackendASP.Models;

[Serializable]
public struct CreateAndSendTokensPayload
{
    public string InitialAccount { get; set; }
    public string OwnerAccount { get; set; }
    public string MintAccount { get; set; }
}