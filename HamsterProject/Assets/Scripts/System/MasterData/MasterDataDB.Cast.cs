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
}
