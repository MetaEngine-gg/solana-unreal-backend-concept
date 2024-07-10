using System.Text;
using Solnet.Rpc;
using Solnet.Rpc.Core.Http;
using Solnet.Rpc.Models;
using Solnet.Serum;
using Solnet.Serum.Models;

namespace Core.Data_Controllers;

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
            .AppendLine("OpenOrdersAccount::")
            .AppendLine($"Owner: {account.Owner.Key}")
            .AppendLine($"Market: {account.Market.Key}")
            .AppendLine($"BaseTotal: {account.BaseTokenTotal}")
            .AppendLine($"BaseFree: {account.BaseTokenFree}")
            .AppendLine($"QuoteTotal: {account.QuoteTokenTotal}")
            .AppendLine($"QuoteFree: {account.QuoteTokenFree}")
            .AppendLine();

        foreach (var order in account.Orders)
        {
            stringBuilder.AppendLine("OpenOrder::")
                .AppendLine($"IsBid: {order.IsBid}")
                .AppendLine($"Price: {order.RawPrice}")
                .AppendLine();
        }

        return (true, stringBuilder.ToString());
    }

    public (bool result , string text) FindOpenOrdersAccounts(string marketAddress, string ownerAddress)
    {
        // Get the market account data.
        var market = _serumClient.GetMarket(marketAddress);
        if (market == null)
        {
            return (false, "Failed to get market by market address");
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
            var ooa = OpenOrdersAccount.Deserialize(Convert.FromBase64String(account.Account.Data[0]));
            
            status.AppendLine("---------------------");
            status.AppendLine($"OpenOrdersAccount:: Owner: {ooa.Owner.Key} Market: {ooa.Market.Key}");
            status.AppendLine();
            status.AppendLine($"OpenOrdersAccount:: Owner: {ooa.Owner.Key} Market: {ooa.Market.Key}");
            status.AppendLine($"BaseTotal: {ooa.BaseTokenTotal} BaseFree: {ooa.BaseTokenFree}");
            status.AppendLine($"QuoteTotal: {ooa.QuoteTokenTotal} QuoteFree: {ooa.QuoteTokenFree}");
            status.AppendLine("---------------------");
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
            status.AppendLine("---------------------");
            status.AppendLine($"OpenOrder::");
            status.AppendLine($"Bid: {openOrder.IsBid}");
            status.AppendLine($"Price: {openOrder.RawPrice}");
            status.AppendLine($"Quantity: {openOrder.RawQuantity}");
            status.AppendLine($"OrderId: {openOrder.OrderId}");
            status.AppendLine($"ClientOrderId: {openOrder.ClientOrderId}");
            status.AppendLine("---------------------");
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