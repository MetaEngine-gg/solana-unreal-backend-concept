using Microsoft.AspNetCore.Mvc;
using SolanaNetBackendASP.Data_Controllers;

namespace SolanaNetBackendASP.Controllers;

[ApiController, Route("api/solana-serum")]
public class SolanaSerumController : ControllerBase
{
    private readonly SolnetSerumDataController _solnetSerumDataController;

    public SolanaSerumController(SolnetSerumDataController programsDataController)
    {
        _solnetSerumDataController = programsDataController;
    }

    [HttpGet, Route("get-open-orders")]
    public ActionResult<string> GetOpenOrders(string openOrdersAccountAddress)
    {
        if (string.IsNullOrEmpty(openOrdersAccountAddress))
        {
            return StatusCode(400, "Open orders account address is required.");
        }
        
        var payload = _solnetSerumDataController.GetOpenOrders(openOrdersAccountAddress);
        return payload.result ? StatusCode(200, payload.text) : StatusCode(500, payload.text);
    }
    
    [HttpGet, Route("find-open-orders-accounts")]
    public ActionResult<string> FindOpenOrdersAccounts(string marketAddress, string ownerAddress)
    {
        if (string.IsNullOrEmpty(marketAddress))
        {
            return StatusCode(400, "Market address is required.");
        }
        
        if (string.IsNullOrEmpty(ownerAddress))
        {
            return StatusCode(400, "Owner address is required.");
        }
        
        var payload = _solnetSerumDataController.FindOpenOrdersAccounts(marketAddress, ownerAddress);
        return payload.result ? StatusCode(200, payload.text) : StatusCode(500, payload.text);
    }
    
    [HttpGet, Route("get-token-mints")]
    public ActionResult<string> GetTokenMints()
    {
        var payload = _solnetSerumDataController.GetTokenMints();
        return payload.result ? StatusCode(200, payload.text) : StatusCode(500, payload.text);
    }
}