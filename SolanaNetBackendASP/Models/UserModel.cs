using Solnet.Wallet;

public struct UserModel
{
    public string Name { get; set; }
    public Wallet Wallet { get; init; }
}