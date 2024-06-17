using Solnet.Rpc;

namespace SolanaNetBackendASP.Data_Controllers;

public class SolanaRpc : IDisposable
{
    private readonly ILogger<SolanaRpc> _logger;
    private readonly IRpcClient _rpcClient;
    private readonly IStreamingRpcClient _streamingRpcClient;
    private readonly UserDataController _userController;

    public SolanaRpc(ILogger<SolanaRpc> logger, UserDataController userController) 
    {
        _logger = logger;
        _userController = userController;

        _rpcClient = ClientFactory.GetClient(Cluster.TestNet);
        _streamingRpcClient = ClientFactory.GetStreamingClient(Cluster.TestNet);
        _streamingRpcClient.ConnectAsync().Wait();

        SubscribeToBalanceChanges();
    }

    public void Dispose()
    {
        _streamingRpcClient.Dispose();
    }

    private void SubscribeToBalanceChanges()
    {
        _streamingRpcClient.SubscribeAccountInfo(_userController.Model.Wallet.Account.PublicKey, (sender, info) =>
        {
            _logger.LogInformation($"Balance changed: {info.Value.Lamports}");
        });
    }
    
    public bool RequestAirDrop(string address, ulong amount)
    {
        var transactionHash = _rpcClient.RequestAirdrop(address, amount);
        var message = $"Air Drop Result: {transactionHash.WasSuccessful}";
        _logger.LogInformation(message);

        return transactionHash.WasSuccessful;
    }
}
