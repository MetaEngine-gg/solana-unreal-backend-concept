using Solnet.Wallet;

namespace Core.Models.User;

[Serializable]
public class UserModel
{
    public string Name { get; set; }
    public Wallet Wallet { get; init; }
}