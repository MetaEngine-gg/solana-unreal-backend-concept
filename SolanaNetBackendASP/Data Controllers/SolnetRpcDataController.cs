using Solnet.Rpc;

namespace SolanaNetBackendASP.Data_Controllers;

public class SolnetRpcDataController : IDisposable
{
    private readonly ILogger<SolnetRpcDataController> _logger;
    private readonly IRpcClient _rpcClient;
    private readonly IStreamingRpcClient _streamingRpcClient;
    private readonly UserDataController _userController;

    public SolnetRpcDataController(ILogger<SolnetRpcDataController> logger, UserDataController userController) 
    {
        _logger = logger;
        _userController = userController;

        _rpcClient = ClientFactory.GetClient(Cluster.TestNet);
        _streamingRpcClient = ClientFactory.GetStreamingClient(Cluster.TestNet);
        _streamingRpcClient.ConnectAsync().Wait();

        SubscribeToBalanceChanges("08x31234124231432144"); // example wallet address, just placeholder
    }

    public void Dispose()
    {
        _streamingRpcClient.Dispose();
    }

    private void SubscribeToBalanceChanges(string walletAddress)
    {
        var wallet = _userController.Model.Users.TryGetValue(walletAddress, out var user) ? user.Wallet : null;
        if (wallet == null)
        {
            _logger.LogError("Wallet not found for address: {WalletAddress}", walletAddress);
            return;
        }
        
        _streamingRpcClient.SubscribeAccountInfo(wallet.Account.PublicKey, (sender, info) =>
        {
            _logger.LogInformation("Balance changed: {ValueLamports}", info.Value.Lamports);
        });
    }
    
    public bool RequestAirDrop(string walletAddress, ulong amount)
    {
        var transactionHash = _rpcClient.RequestAirdrop(walletAddress, amount);
        var message = $"Air Drop Result: {transactionHash.WasSuccessful}";
        _logger.LogInformation(message);

        return transactionHash.WasSuccessful;
    }
}
