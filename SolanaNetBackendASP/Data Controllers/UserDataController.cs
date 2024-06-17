using SolanaNetBackendASP.Data_Controllers.Components;
using Solnet.Rpc;

namespace SolanaNetBackendASP.Data_Controllers;

public class UserDataController
{
    private readonly ILogger<UserDataController> _logger;
    private readonly IRpcClient _rpcClient;
    private readonly IStreamingRpcClient _streamingRpcClient;

    public UserModel Model { get; private set; }

    public UserDataController(ILogger<UserDataController> logger)
    {
        _logger = logger;

        _rpcClient = ClientFactory.GetClient(Cluster.TestNet);
        _streamingRpcClient = ClientFactory.GetStreamingClient(Cluster.TestNet);
        _streamingRpcClient.ConnectAsync().Wait();

        CreateUser();
    }

    #region Init

    public void CreateUser()
    {
        Model = new UserModel
        {
            Name = $"User #{new Random().Next()}",
            Wallet = SolnetWallet.CreateWallet()
        };

        _logger.LogInformation("Wallet: {AccountPublicKey}", Model.Wallet.Account.PublicKey);
        _logger.LogInformation("Created User: {ModelName}", Model.Name);
    }

    #endregion

    public async Task<Solnet.Rpc.Core.Http.RequestResult<Solnet.Rpc.Messages.ResponseValue<ulong>>> GetBalance(string address)
    {
        var result = await _rpcClient.GetBalanceAsync(address);
        return result;
    }
}