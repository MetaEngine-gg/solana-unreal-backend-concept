﻿using SolanaNetBackendASP.Data_Controllers.Components;
using SolanaNetBackendASP.Models;
using Solnet.Rpc;
using Solnet.Rpc.Core.Http;
using Solnet.Rpc.Messages;

namespace SolanaNetBackendASP.Data_Controllers;

public class UserDataController : IDisposable
{
    private readonly ILogger _logger;
    private readonly IRpcClient _rpcClient;
    private readonly IStreamingRpcClient _streamingRpcClient;

    public UsersContainer Model { get; private set; } = new();

    public UserDataController(ILogger logger)
    {
        _logger = logger;

        _rpcClient = ClientFactory.GetClient(Cluster.TestNet);
        _streamingRpcClient = ClientFactory.GetStreamingClient(Cluster.TestNet);
        _streamingRpcClient.ConnectAsync().Wait();

        CreateUser();
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

    public async Task<RequestResult<ResponseValue<ulong>>> GetBalance(string address)
    {
        var result = await _rpcClient.GetBalanceAsync(address);
        return result;
    }

    public string GetPrivateKey(string address)
    {
        var result = Model.Users.TryGetValue(address, out var user);
        if (!result)
        {
            _logger.LogError("User not found for address: {Address}", address);
            return string.Empty;
        }

        return user.Wallet.Account.PrivateKey;
    }
    
    public string GetPublicKey(string address)
    {
        var result = Model.Users.TryGetValue(address, out var user);
        if (!result)
        {
            _logger.LogError("User not found for address: {Address}", address);
            return string.Empty;
        }

        return user.Wallet.Account.PublicKey;
    }

    #endregion
}