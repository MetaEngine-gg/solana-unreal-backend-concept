using Solnet.Wallet;

namespace SolBridge.Models.User;

[Serializable]
public class UserModel
{
    public string Name { get; set; }
    public Wallet Wallet { get; init; }
}