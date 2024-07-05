using System.Text;
using Solnet.Rpc;
using Solnet.Serum;
using ClientFactory = Solnet.Serum.ClientFactory;

namespace SolanaNetBackendASP.Data_Controllers;

public class SolnetSerumDataController
{
    private readonly ILogger<SolnetSerumDataController> _logger;
    private readonly ISerumClient _serumClient;

    // Public key for Open Orders Account.
    private const string OpenOrdersAccountAddress = "4beBRAZSVcCm7jD7yAmizqqVyi39gVrKNeEPskickzSF";

    public SolnetSerumDataController(ILogger<SolnetSerumDataController> logger)
    {
        _logger = logger;
        _serumClient = ClientFactory.GetClient(Cluster.TestNet);
    }

    public bool GetOpenOrders()
    {
        var account = _serumClient.GetOpenOrdersAccount(OpenOrdersAccountAddress);
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
            .Append(account.QuoteTokenFree);
        
        _logger.LogInformation(stringBuilder.ToString());

        foreach (var order in account.Orders)
        {
            _logger.LogInformation("OpenOrder:: IsBid: {OrderIsBid} Price: {OrderRawPrice}", order.IsBid, order.RawPrice);
        }

        return true;
    }
}