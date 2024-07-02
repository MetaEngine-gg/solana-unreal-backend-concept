using Solnet.Programs;
using Solnet.Rpc;
using Solnet.Rpc.Builders;

namespace SolanaNetBackendASP.Data_Controllers;

public class SolnetProgramsDataController
{
    private readonly ILogger<SolnetProgramsDataController> _logger;
    private readonly UserDataController _userDataController;
    private readonly IRpcClient _rpcClient;

    public SolnetProgramsDataController(ILogger<SolnetProgramsDataController> logger, 
        UserDataController userDataController, IRpcClient rpcClient)
    {
        _logger = logger;
        _userDataController = userDataController;
        _rpcClient = rpcClient;
    }

    public bool RunHelloWorldProgram(string walletAddress)
    {
        if (_userDataController.Model.Users.TryGetValue(walletAddress, out var userModel) == false)
        {
            _logger.LogError("Wallet not found");
            return false;
        }

        var wallet = userModel.Wallet;
        var memoInstruction = MemoProgram.NewMemo(wallet.Account, "Hello Solana World, using Solnet :)");

        var recentHash = _rpcClient.GetLatestBlockHash();

        var tx = new TransactionBuilder().
            SetFeePayer(wallet.Account).
            AddInstruction(memoInstruction).
            SetRecentBlockHash(recentHash.Result.Value.Blockhash).
            Build(wallet.Account);

        _logger.LogInformation("Tx: {Tx}", tx);

        return true;
    }
}