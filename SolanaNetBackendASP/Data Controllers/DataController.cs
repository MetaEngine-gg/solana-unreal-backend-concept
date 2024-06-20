using Newtonsoft.Json;
using SolanaNetBackendASP.Models;

namespace SolanaNetBackendASP.Data_Controllers;

public class DataController
{
    private const string DataFolder = "Data";
    private const string UsersDataFile = "usersData.json";
    
    private readonly ILogger<DataController> _logger;
    private readonly UserDataController _userDataController;
    
    public UserDataController UserDataController => _userDataController;
    
    public DataController(ILogger<DataController> logger, UserDataController userDataController)
    {
        _logger = logger;
        _userDataController = userDataController;
        
        _userDataController.Model = LoadData(GetCurrentSavePath());
    }

    #region IO

    private string GetCurrentSavePath()
    {
        if (Directory.Exists(DataFolder) == false)
        {
            Directory.CreateDirectory(DataFolder);
        }

        var path = Path.Combine(DataFolder, UsersDataFile);
        return path;
    }
    
    private void SaveData()
    {
        var path = GetCurrentSavePath();
        var jsonFile = JsonConvert.SerializeObject(_userDataController.Model, Formatting.Indented);
        File.WriteAllText(path, jsonFile);

        _logger.LogInformation("Saved table data to: {Path}", path);
    }

    public UsersContainer LoadData(string path)
    {
        if (!File.Exists(path))
        {
            _logger.LogInformation("No file to load");
            return new UsersContainer();
        }

        using var reader = new StreamReader(path);
        var fileContent = reader.ReadToEnd();

        var savedModel = JsonConvert.DeserializeObject<UsersContainer>(fileContent);
        if (savedModel != null)
        {
            _logger.LogInformation("Loaded table data from: {Path}", path);
            return savedModel;
        }

        File.Copy(path, path + ".broken"); // copy broken file
        _logger.LogError("Data Load Failed");
        return new UsersContainer();
    }

    #endregion
}