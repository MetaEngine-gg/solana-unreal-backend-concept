﻿using Solnet.Programs;
using Solnet.Rpc;
using Solnet.Rpc.Builders;
using Solnet.Rpc.Models;

namespace Core.Data_Controllers;

public class SolnetRpcDataController : IDisposable
{
    private readonly ILogger<SolnetRpcDataController> _logger;
    private readonly IRpcClient _rpcClient;
    private readonly IStreamingRpcClient _streamingRpcClient;
    private readonly DataController _dataController;

    public SolnetRpcDataController(ILogger<SolnetRpcDataController> logger, DataController dataController) 
    {
        _logger = logger;
        _dataController = dataController;

        _rpcClient = ClientFactory.GetClient(Cluster.TestNet);
        _streamingRpcClient = ClientFactory.GetStreamingClient(Cluster.TestNet);
        _streamingRpcClient.ConnectAsync().Wait();

        SubscribeToBalanceChanges("5omQJtDUHA3gMFdHEQg1zZSvcBUVzey5WaKWYRmqF1Vj"); // example wallet address, just placeholder
    }

    public void Dispose()
    {
        _streamingRpcClient.Dispose();
    }

    private void SubscribeToBalanceChanges(string walletAddress)
    {
        var wallet = _dataController.UserDataController.Model.Users.TryGetValue(walletAddress, out var user) ? user.Wallet : null;
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

    public async Task<(bool result , AccountInfo accountInfo)> GetAccountInfo(string address)
    {
        var request = await _rpcClient.GetAccountInfoAsync(address);
        if (!request.WasSuccessful)
        {
            _logger.LogError("Account info not found for address: {Address}", address);
            return (false, new AccountInfo());
        }
        
        return (request.WasSuccessful, request.Result.Value);
    }
    
    public async Task<(bool result , string text)> SendTransaction(string fromAddress, string toAddress, ulong amount)
    {
        var fromWallet = _dataController.UserDataController.Model.Users.TryGetValue(fromAddress, out var fromUser) ? fromUser.Wallet : null;
        var toWallet = _dataController.UserDataController.Model.Users.TryGetValue(toAddress, out var toUser) ? toUser.Wallet : null;

        if (fromWallet == null)
        {
            _logger.LogError("Wallet not found for address: {FromAddress}", fromAddress);
            return (false, $"Wallet not found for address: {fromAddress}");
        }
        
        if (toWallet == null)
        {
            _logger.LogError("Wallet not found for address: {ToAddress}", toAddress);
            return (false, $"Wallet not found for address: {toAddress}");
        }
        
        // Get a recent block hash to include in the transaction
        var blockHash = await _rpcClient.GetLatestBlockHashAsync();

        // Initialize a transaction builder and chain as many instructions as you want before building the message
        var tx = new TransactionBuilder().
            SetRecentBlockHash(blockHash.Result.Value.Blockhash).
            SetFeePayer(fromWallet.Account.PublicKey).
            AddInstruction(MemoProgram.NewMemo(fromWallet.Account.PublicKey, "Hello from Sol.Net :)")).
            AddInstruction(SystemProgram.Transfer(fromWallet.Account.PublicKey, toWallet.Account.PublicKey, amount)).
            Build(fromWallet.Account);

        var transaction = await _rpcClient.SendTransactionAsync(tx);
        return transaction.WasSuccessful ? (true, "Transaction completed successfully") : (false, $"Error Code: {transaction.ServerErrorCode}");
    }
}
