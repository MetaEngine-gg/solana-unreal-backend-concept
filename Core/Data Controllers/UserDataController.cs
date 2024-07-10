using SolBridge.Data_Controllers.Components;
using SolBridge.Models.User;
using Solnet.Rpc;

namespace SolBridge.Data_Controllers;

public class UserDataController : IDisposable
{
    private readonly ILogger _logger;
    private readonly IRpcClient _rpcClient;
    private readonly IStreamingRpcClient _streamingRpcClient;

    public UsersContainer Model { get; set; } = new();

    public UserDataController(ILogger<UserDataController> logger)
    {
        _logger = logger;

        _rpcClient = ClientFactory.GetClient(Cluster.TestNet);
        _streamingRpcClient = ClientFactory.GetStreamingClient(Cluster.TestNet);
        _streamingRpcClient.ConnectAsync().Wait();
    }
    
    public void Dispose()
    {
        _streamingRpcClient.Dispose();
    }

    #region User Creation

    public void CreateUser()
    {
        var user = new UserModel
        {
            Name = $"User #{new Random().Next()}",
            Wallet = SolnetWallet.CreateWallet()
        };
        Model.Users.Add(user.Wallet.Account.PublicKey, user);
        
        _logger.LogInformation("Wallet: {AccountPublicKey}", user.Wallet.Account.PublicKey);
        _logger.LogInformation("Created User: {ModelName}", user.Name);
    }

    #endregion

    #region Wallet Operations

    public (bool result, string text) GetPublicKey(string address)
    {
        var userModel = Model.Users.FirstOrDefault(pair => pair.Value.Name == address).Value;
        if (userModel == null)
        {
            _logger.LogError("User not found for address: {Address}", address);
            return (false, $"User not found: {address}");
        }

        return (true, $"Public Key: {userModel.Wallet.Account.PublicKey}");
    }
    
    public (bool result, string text) GetPrivateKey(string address)
    {
        var result = Model.Users.TryGetValue(address, out var user);
        if (!result)
        {
            _logger.LogError("User not found for address: {Address}", address);
            return (false, $"User not found: {address}");
        }

        return (true, $"Private Key: {user.Wallet.Account.PrivateKey}");
    }
    
    public async Task<(bool result, string text)> GetBalance(string address)
    {
        var result = await _rpcClient.GetBalanceAsync(address);
        if (!result.WasSuccessful)
        {
            _logger.LogError("Failed to retrieve balance for address: {Address}", address);
            return (false, $"Failed to retrieve balance for address: {address}");
        }

        return (true, $"Balance: {result.Result.Value}");
    }

    #endregion
}