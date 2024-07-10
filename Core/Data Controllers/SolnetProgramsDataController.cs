using System.Text;
using SolBridge.Models;
using Solnet.Extensions;
using Solnet.Programs;
using Solnet.Rpc;
using Solnet.Rpc.Builders;
using Solnet.Wallet;

namespace SolBridge.Data_Controllers;

public class SolnetProgramsDataController
{
    private readonly ILogger<SolnetProgramsDataController> _logger;
    private readonly UserDataController _userDataController;
    private readonly IRpcClient _rpcClient;

    public SolnetProgramsDataController(ILogger<SolnetProgramsDataController> logger,
        UserDataController userDataController)
    {
        _logger = logger;
        _userDataController = userDataController;
        _rpcClient = ClientFactory.GetClient(Cluster.TestNet);
    }

    public (bool result, string text) RunHelloWorldProgram(string walletAddress)
    {
        if (_userDataController.Model.Users.TryGetValue(walletAddress, out var userModel) == false)
        {
            _logger.LogError("Wallet not found");
            return (false, "Wallet not found");
        }

        var wallet = userModel.Wallet;
        var memoInstruction = MemoProgram.NewMemo(wallet.Account, "Hello Solana World, using Solnet :)");

        var recentHash = _rpcClient.GetLatestBlockHash();

        var tx = new TransactionBuilder().SetFeePayer(wallet.Account).AddInstruction(memoInstruction)
            .SetRecentBlockHash(recentHash.Result.Value.Blockhash).Build(wallet.Account);

        _logger.LogInformation("Tx: {Tx}", tx);
        return (true, "Success");
    }

    public (bool result, string text) CreateAndSendTokensToAccount(CreateAndSendTokensPayload payload)
    {
        if (_userDataController.Model.Users.TryGetValue(payload.InitialAccount, out var initialAccountModel) == false)
        {
            _logger.LogError("Initial account not found");
            return (false, "Initial account not found");
        }

        if (_userDataController.Model.Users.TryGetValue(payload.OwnerAccount, out var ownerAccountModel) == false)
        {
            _logger.LogError("Owner account not found");
            return (false, "Owner account not found");
        }

        if (_userDataController.Model.Users.TryGetValue(payload.MintAccount, out var mintAccountModel) == false)
        {
            _logger.LogError("Mint account not found");
            return (false, "Mint account not found");
        }

        var recentHash = _rpcClient.GetLatestBlockHash();

        // By taking someone's address, derive the associated token account for their address and a corresponding mint
        // NOTE: You should check if that person already has an associated token account for that mint!
        PublicKey associatedTokenAccountOwner = new("65EoWs57dkMEWbK4TJkPDM76rnbumq7r3fiZJnxggj2G");
        var associatedTokenAccount =
            AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(associatedTokenAccountOwner,
                mintAccountModel.Wallet.Account);

        var txBytes = new TransactionBuilder().SetRecentBlockHash(recentHash.Result.Value.Blockhash)
            .SetFeePayer(ownerAccountModel.Wallet.Account).AddInstruction(
                AssociatedTokenAccountProgram.CreateAssociatedTokenAccount(
                    ownerAccountModel.Wallet.Account,
                    associatedTokenAccountOwner,
                    mintAccountModel.Wallet.Account)).AddInstruction(TokenProgram.Transfer(
                initialAccountModel.Wallet.Account,
                associatedTokenAccount,
                25000,
                ownerAccountModel.Wallet.Account)). // the ownerAccount was set as the mint authority
            AddInstruction(MemoProgram.NewMemo(ownerAccountModel.Wallet.Account, "Hello from Sol.Net"))
            .Build(new List<Account> { ownerAccountModel.Wallet.Account });

        var signature = _rpcClient.SendTransaction(txBytes);
        _logger.LogInformation("Signature: {Signature}", signature.Result);
        
        return (true, signature.Result);
    }

    public (bool result, string text) DisplayTokenBalancesOfWallet(string walletAddress)
    {
        if (_userDataController.Model.Users.TryGetValue(walletAddress, out var ownerAccountModel) == false)
        {
            _logger.LogError("Wallet not found");
            return (false, "Wallet not found");
        }

        var status = new StringBuilder();
        
        // load Solana token list and get RPC client
        var tokens = TokenMintResolver.Load();
        var client = ClientFactory.GetClient(Cluster.MainNet);

        // load snapshot of wallet and sub-accounts
        var tokenWallet = TokenWallet.Load(client, tokens, ownerAccountModel.Wallet.Account);
        var balances = tokenWallet.Balances();

        // show individual token accounts
        var maxSum = balances.Max(x => x.Symbol.Length);
        var maxName = balances.Max(x => x.TokenName.Length);
        _logger.LogInformation("Individual Accounts...");
        status.AppendLine("Individual Accounts...");

        foreach (var account in tokenWallet.TokenAccounts())
        {
            var message = $"{account.Symbol.PadRight(maxSum)} {account.QuantityDecimal,14} {account.TokenName.PadRight(maxName)} {account.PublicKey} {(account.IsAssociatedTokenAccount ? "[ATA]" : "")}";
            _logger.LogInformation(message);
            status.AppendLine(message);
        }

        return (true, status.ToString());
    }
}