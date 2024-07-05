using Solnet.Rpc;

namespace SolanaNetBackendASP.Data_Controllers;

public class SolnetSerumDataController
{
    private readonly ILogger<SolnetSerumDataController> _logger;
    private readonly UserDataController _userDataController;
    private readonly IRpcClient _rpcClient;

    public SolnetSerumDataController(ILogger<SolnetSerumDataController> logger, UserDataController userDataController)
    {
        _logger = logger;
        _userDataController = userDataController;
        _rpcClient = ClientFactory.GetClient(Cluster.TestNet);
    }

    public bool GetOpenOrders()
    {
        return true;
    }
}