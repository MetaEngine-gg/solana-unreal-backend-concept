using System.Text;
using Solnet.Rpc;
using Solnet.Rpc.Core.Http;
using Solnet.Rpc.Models;
using Solnet.Serum;
using Solnet.Serum.Models;

namespace SolanaNetBackendASP.Data_Controllers;

public class SolnetSerumDataController
{
    private readonly ISerumClient _serumClient;
    private readonly IRpcClient _rpcClient;

    public SolnetSerumDataController()
    {
        _serumClient = Solnet.Serum.ClientFactory.GetClient(Cluster.TestNet);
        _rpcClient = Solnet.Rpc.ClientFactory.GetClient(Cluster.TestNet);
    }

    public (bool result , string text) GetOpenOrders(string openOrdersAccountAddress)
    {
        var account = _serumClient.GetOpenOrdersAccount(openOrdersAccountAddress);
        if (account == null)
        {
            return (false, "Failed to get Open Orders Account");
        }

        var stringBuilder = new StringBuilder();
        stringBuilder
            .Append("OpenOrdersAccount:: Owner: ")
            .Append(account.Owner.Key)
            .Append(" Market: ")
            .Append(account.Market.Key)
            .Append('\n')
            .Append("BaseTotal: ")
            .Append(account.BaseTokenTotal)
            .Append(" BaseFree: ")
            .Append(account.BaseTokenFree)
            .Append('\n')
            .Append("QuoteTotal: ")
            .Append(account.QuoteTokenTotal)
            .Append(" QuoteFree: ")
            .Append(account.QuoteTokenFree)
            .Append('\n');

        foreach (var order in account.Orders)
        {
            stringBuilder.Append($"OpenOrder:: IsBid: {order.IsBid} Price: {order.RawPrice}");
        }

        return (true, stringBuilder.ToString());
    }

    public (bool result , string text) FindOpenOrdersAccounts(string marketAddress, string ownerAddress)
    {
        // Get the market account data.
        var market = _serumClient.GetMarket(marketAddress);
        if (market == null)
        {
            return (false, "Failed to get Market");
        }

        var status = new StringBuilder();
        
        // Get open orders accounts for a market.
        List<MemCmp> filters =
        [
            new MemCmp { Offset = 13, Bytes = marketAddress },
            new MemCmp { Offset = 45, Bytes = ownerAddress }
        ];
        RequestResult<List<AccountKeyPair>> accounts =
            _rpcClient.GetProgramAccounts(SerumProgram.MainNetProgramIdKeyV3,
                dataSize: OpenOrdersAccount.Layout.SpanLength, memCmpList: filters);
        if (accounts.Result.Count == 0)
        {
            return (false, "No Open Orders Accounts found");
        }

        // Print all the found open orders accounts
        foreach (var account in accounts.Result)
        {
            status.Append($"---------------------");
            // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
            status.Append($"OpenOrdersAccount: {account.PublicKey} - Owner: {account.Account.Owner}");
            var ooa = OpenOrdersAccount.Deserialize(Convert.FromBase64String(account.Account.Data[0]));
            status.Append($"OpenOrdersAccount:: Owner: {ooa.Owner.Key} Market: {ooa.Market.Key}\n" +
                                   $"BaseTotal: {ooa.BaseTokenTotal} BaseFree: {ooa.BaseTokenFree}\n" +
                                   $"QuoteTotal: {ooa.QuoteTokenTotal} QuoteFree: {ooa.QuoteTokenFree}");
            status.Append($"---------------------");
        }

        var openOrdersAddress = accounts.Result[0].PublicKey;
        var openOrdersAccount =
            OpenOrdersAccount.Deserialize(Convert.FromBase64String(accounts.Result[0].Account.Data[0]));

        // Get both sides of the order book
        var bidSide = _serumClient.GetOrderBookSide(market.Bids.Key);
        var askSide = _serumClient.GetOrderBookSide(market.Asks.Key);

        List<OpenOrder> asks = askSide.GetOrders();
        foreach (var ask in asks.Where(ask => ask.Owner.Key == openOrdersAddress))
        {
            openOrdersAccount.Orders[ask.OrderIndex].RawQuantity = ask.RawQuantity;
        }

        List<OpenOrder> bids = bidSide.GetOrders();
        foreach (var bid in bids.Where(ask => ask.Owner.Key == openOrdersAddress))
        {
            openOrdersAccount.Orders[bid.OrderIndex].RawQuantity = bid.RawQuantity;
        }

        foreach (var openOrder in openOrdersAccount.Orders)
        {
            status.Append($"OpenOrder:: Bid: {openOrder.IsBid}\t" +
                                   $"Price: {openOrder.RawPrice}\t" +
                                   $"Quantity: {openOrder.RawQuantity}\t" +
                                   $"OrderId: {openOrder.OrderId}\t" +
                                   $"ClientOrderId: {openOrder.ClientOrderId}");
        }

        return (true, status.ToString());
    }

    public (bool result , string text) GetTokenMints()
    {
        var status = new StringBuilder();
        status.AppendLine("Token Mints:");
        
        var res = _serumClient.GetTokens();
        foreach (var tokenInfo in res)
        {
            status.AppendLine($"TokenInfo :: Name: {tokenInfo.Name} :: Address: {tokenInfo.Address.Key}");
        }

        return (true, status.ToString());
    }
}