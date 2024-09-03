using System.Collections;
using System.Collections.Generic;

/// <summary>
/// MasterDataDB data cast. csv to MasterData.
/// </summary>
public partial class MasterDataDB
{
    /// <summary>
    /// Get AreCardMaster.
    /// </summary>
    /// <param name="masterName"></param>
    /// <returns></returns>
    /*
    private Dictionary<int, AreCardMaster> GetAreCardMaster(string masterName)
    {
        Dictionary<int, AreCardMaster> result = new Dictionary<int, AreCardMaster>();

        List<string[]> csvData = ImportMasterCsv(masterName);
        foreach (string[] line in csvData)
        {
            int Id = int.Parse(line[0]);
            result.Add(Id, new AreCardMaster
            {
                Id = Id,
                AreWord = line[1]
            });
        }

        return result;
    }
    */

    /// <summary>
    /// Get WordCardMaster.
    /// </summary>
    /// <param name="masterName"></param>
    /// <returns></returns>
    /*
    private Dictionary<int, WordCardMaster> GetWordCardMaster(string masterName)
    {
        Dictionary<int, WordCardMaster> result = new Dictionary<int, WordCardMaster>();

        List<string[]> csvData = ImportMasterCsv(masterName);
        foreach (string[] line in csvData)
        {
            int Id = int.Parse(line[0]);
            result.Add(Id, new WordCardMaster
            {
                Id = Id,
                Words = new List<string>()
                {
                    line[1],
                    line[2],
                    line[3],
                    line[4],
                    line[5],
                    line[6],
                }
            });
        }

        return result;
    }
    */
    
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
            result.Add(Id, new HamsterMaster{
                HamsterID = Id,
                Name = line[1],
                ImagePath = line[2],
                BugID = int.Parse(line[3]),
                BugFixTime = float.Parse(line[4]),
            });
        }

        return result;
    }
}
