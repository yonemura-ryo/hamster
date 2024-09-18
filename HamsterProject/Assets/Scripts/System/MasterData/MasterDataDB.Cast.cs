using System.Collections;
using System.Collections.Generic;

/// <summary>
/// MasterDataDB data cast. csv to MasterData.
/// </summary>
public partial class MasterDataDB
{
    /// <summary>
    /// Get HamsterMaster.
    /// </summary>
    /// <param name="masterName"></param>
    /// <returns></returns>
    private Dictionary<int, HamsterMaster> GetHamsterMaster(string masterName)
    {
        Dictionary<int, HamsterMaster> result = new Dictionary<int, HamsterMaster>();

        List<string[]> csvData = ImportMasterCsv(masterName);
        foreach (string[] line in csvData)
        {
            int Id = int.Parse(line[0]);
            result.Add(Id, new HamsterMaster(
                Id,
                line[1],
                line[2],
                int.Parse(line[3]),
                float.Parse(line[4])
            ));
        }

        return result;
    }
    
    private Dictionary<int, FacilityMaster> GetFacilityMaster(string masterName)
    {
        Dictionary<int, FacilityMaster> result = new Dictionary<int, FacilityMaster>();

        List<string[]> csvData = ImportMasterCsv(masterName);
        foreach (string[] line in csvData)
        {
            int Id = int.Parse(line[0]);
            result.Add(Id, new FacilityMaster(
                Id,
                line[1],
                int.Parse(line[2])
            ));
        }

        return result;
    }
    private Dictionary<string, FacilityLevelMaster> GetFacilityLevelMaster(string masterName)
    {
        Dictionary<string, FacilityLevelMaster> result = new Dictionary<string, FacilityLevelMaster>();

        List<string[]> csvData = ImportMasterCsv(masterName);
        foreach (string[] line in csvData)
        {
            string Id = line[0];
            string Level = line[1];
            result.Add(Id + ":" + Level, new FacilityLevelMaster(
                int.Parse(Id),
                int.Parse(Level),
                int.Parse(line[2])
            ));
        }

        return result;
    }
    
    private Dictionary<int, FoodMaster> GetFoodMaster(string masterName)
    {
        Dictionary<int, FoodMaster> result = new Dictionary<int, FoodMaster>();

        List<string[]> csvData = ImportMasterCsv(masterName);
        foreach (string[] line in csvData)
        {
            int Id = int.Parse(line[0]);
            result.Add(Id, new FoodMaster(
                Id,
                line[1],
                int.Parse(line[2]),
                float.Parse(line[3])
            ));
        }

        return result;
    }
}
