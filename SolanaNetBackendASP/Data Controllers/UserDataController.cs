using SolanaNetBackendASP.Data_Controllers.Components;
using SolanaNetBackendASP.Models;
using Solnet.Rpc;

namespace SolanaNetBackendASP.Data_Controllers;

public class UserDataController
{
    private readonly ILogger<DataController> _logger;
    private readonly IRpcClient _rpcClient;
    private readonly IStreamingRpcClient _streamingRpcClient;

    public UsersContainer Model { get; private set; }

    public UserDataController(ILogger<DataController> logger)
    {
        _logger = logger;

        _rpcClient = ClientFactory.GetClient(Cluster.TestNet);
        _streamingRpcClient = ClientFactory.GetStreamingClient(Cluster.TestNet);
        _streamingRpcClient.ConnectAsync().Wait();

        CreateUser();
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

    public async Task<Solnet.Rpc.Core.Http.RequestResult<Solnet.Rpc.Messages.ResponseValue<ulong>>> GetBalance(string address)
    {
        var result = await _rpcClient.GetBalanceAsync(address);
        return result;
    }

    public string GetPrivateKey(string address)
    {
        return Model.Users.TryGetValue(address, out var user) ? user.Wallet.Account.PrivateKey : string.Empty;
    }
}