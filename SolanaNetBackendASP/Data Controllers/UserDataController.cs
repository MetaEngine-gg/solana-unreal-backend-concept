using Solnet.Rpc;
using Solnet.Wallet;
using Solnet.Wallet.Bip39;

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
            Wallet = CreateWallet()
        };

        _logger.LogInformation($"Created User: {Model.Name}");
    }

    private Wallet CreateWallet()
    {
        var wallet = new Wallet(WordCount.TwentyFour, WordList.English);
        _logger.LogInformation($"Wallet: {wallet.Account.PublicKey}");
        return wallet;
    }

    #endregion

    public async Task<Solnet.Rpc.Core.Http.RequestResult<Solnet.Rpc.Messages.ResponseValue<ulong>>> GetBalance(string address)
    {
        var result = await _rpcClient.GetBalanceAsync(address);
        return result;
    }
}