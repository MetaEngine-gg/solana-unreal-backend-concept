using Solnet.Wallet;

namespace SolanaNetBackendASP.Models;

[Serializable]
public struct UserModel
{
    public string Name { get; set; }
    public Wallet Wallet { get; init; }
}