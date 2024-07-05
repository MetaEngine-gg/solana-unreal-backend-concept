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
        
        var result = _solnetSerumDataController.GetOpenOrders(openOrdersAccountAddress);
        return result ? StatusCode(200) : StatusCode(500, "Failed to get open orders.");
    }
}