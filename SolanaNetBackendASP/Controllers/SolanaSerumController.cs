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

    [HttpPost, Route("get-open-orders")]
    public ActionResult<string> EncryptAccountData()
    {
        var result = _solnetSerumDataController.GetOpenOrders();
        return result ? StatusCode(200) : StatusCode(500, "Failed to get open orders.");
    }
}