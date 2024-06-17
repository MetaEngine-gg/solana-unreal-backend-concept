namespace SolanaNetBackendASP.Models;

[Serializable]
public class UsersContainer
{
    public Dictionary<string, UserModel> Users { get; set; } = new();
}