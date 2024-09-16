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
}
