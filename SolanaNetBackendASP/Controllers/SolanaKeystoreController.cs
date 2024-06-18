using Microsoft.AspNetCore.Mvc;
using SolanaNetBackendASP.Data_Controllers;

namespace SolanaNetBackendASP.Controllers;

[ApiController, Route("api/solana-keystore")]
public class SolanaKeystoreController : ControllerBase
{
    private readonly SolnetKeystoreDataController _solnetKeystoreController;

    public SolanaKeystoreController(SolnetKeystoreDataController keystoreController)
    {
        _solnetKeystoreController = keystoreController;
    }
}