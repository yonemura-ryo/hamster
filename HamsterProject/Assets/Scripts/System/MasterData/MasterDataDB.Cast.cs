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
                int.Parse(line[1]),
                line[2],
                line[3],
                line[4],
                int.Parse(line[5]),
                float.Parse(line[6]),
                float.Parse(line[7]),
                int.Parse(line[8]),
                int.Parse(line[9]),
                int.Parse(line[10])
            ));
        }

        return result;
    }

    private Dictionary<int, HamsterBugMaster>GetHamsterBugMaster(string masterName)
    {
        Dictionary<int, HamsterBugMaster> result = new Dictionary<int, HamsterBugMaster>();

        List<string[]> csvData = ImportMasterCsv(masterName);
        foreach (string[] line in csvData)
        {
            int Id = int.Parse(line[0]);
            result.Add(Id, new HamsterBugMaster(
                Id,
                int.Parse(line[1])
            ));
        }

        return result;
    }

    private Dictionary<string, HamsterColorTypeMaster> GetHamsterColorTypeMaster(string masterName)
    {
        Dictionary<string, HamsterColorTypeMaster> result = new Dictionary<string, HamsterColorTypeMaster>();

        List<string[]> csvData = ImportMasterCsv(masterName);
        foreach (string[] line in csvData)
        {
            int Id = int.Parse(line[0]);
            int colorId = int.Parse(line[1]);
            result.Add(Id + ":" + colorId, new HamsterColorTypeMaster(
                Id,
                int.Parse(line[1])
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
                line[2],
                line[3],
                int.Parse(line[4])
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
                int.Parse(line[2]),
                line[3]
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
                line[2],
                int.Parse(line[3]),
                int.Parse(line[4]),
                line[5],
                int.Parse(line[6]),
                //float.Parse(line[7]),
                0,
                //int.Parse(line[8])
                0
            ));
        }

        return result;
    }

    private Dictionary<int, UserRankMaster> GetUserRankMaster(string masterName)
    {
        Dictionary<int, UserRankMaster> result = new Dictionary<int, UserRankMaster>();
        List<string[]> csvData = ImportMasterCsv(masterName);
        foreach (string[] line in csvData)
        {
            int Id = int.Parse(line[0]);
            result.Add(Id, new UserRankMaster(
                Id,
                int.Parse(line[1]),
                int.Parse(line[2]),
                int.Parse(line[3])
            ));
        }

        return result;
    }

    public Dictionary<int, RarityLotteryMaster> GetRarityLotteryMaster(string masterName)
    {
        Dictionary<int, RarityLotteryMaster> result = new Dictionary<int, RarityLotteryMaster>();
        List<string[]> csvData = ImportMasterCsv(masterName);
        foreach (string[] line in csvData)
        {
            int Id = int.Parse(line[0]);
            result.Add(Id, new RarityLotteryMaster(
                Id,
                int.Parse(line[1]),
                int.Parse(line[2]),
                int.Parse(line[3]),
                int.Parse(line[4]),
                int.Parse(line[5])
            ));
        }

        return result;
    }

    public Dictionary<int, MissionMaster> GetMissionMaster(string masterName)
    {
        Dictionary<int, MissionMaster> result = new Dictionary<int, MissionMaster>();
        List<string[]> csvData = ImportMasterCsv(masterName);
        foreach (string[] line in csvData)
        {
            int Id = int.Parse(line[0]);
            result.Add(Id, new MissionMaster(
                Id,
                int.Parse(line[1]),
                int.Parse(line[2]),
                int.Parse(line[3]),
                int.Parse(line[4]),
                int.Parse(line[5]),
                int.Parse(line[6]),
                line[7]
            ));
        }

        return result;
    }
}
