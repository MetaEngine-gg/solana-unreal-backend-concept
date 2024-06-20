namespace SolanaNetBackendASP.Data_Controllers;

public class SolnetProgramsDataController
{
    private readonly ILogger<SolnetProgramsDataController> _logger;

    public SolnetProgramsDataController(ILogger<SolnetProgramsDataController> logger)
    {
        _logger = logger;
    }
}