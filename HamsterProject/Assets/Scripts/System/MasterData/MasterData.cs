using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// マスターデータ取得用クラス
/// </summary>
public static class MasterData
{
    private static MasterDataDB db;
    public static MasterDataDB DB
    {
        get
        {
            if (db != null) return db;
            db = new MasterDataDB();
            return db;
        }
    }
}

/// <summary>
/// マスターデータ格納用クラス
/// </summary>
public partial class MasterDataDB
{
    /// <summary> m_hamster公開用 </summary>
    public IReadOnlyDictionary<int, HamsterMaster> HamsterMaster => hamsterMaster;
    public IReadOnlyDictionary<int, HamsterBugMaster> HamsterBugMaster => hamsterBugMaster;
    public IReadOnlyDictionary<string, HamsterColorTypeMaster> HamsterColorTypeMaster => hamsterColorTypeMaster;
    public IReadOnlyDictionary<int, FacilityMaster> FacilityMaster => facilityMaster;
    public IReadOnlyDictionary<string, FacilityLevelMaster> FacilityLevelMaster => facilityLevelMaster;
    public IReadOnlyDictionary<int, FoodMaster> FoodMaster => foodMaster;
    public IReadOnlyDictionary<int, UserRankMaster> UserRankMaster => userRankMaster;
    public IReadOnlyDictionary<int, RarityLotteryMaster> RarityLotteryMaster => rarityLotteryMaster;

    private Dictionary<int, HamsterMaster> hamsterMaster;
    private Dictionary<int, HamsterBugMaster> hamsterBugMaster;
    private Dictionary<string, HamsterColorTypeMaster> hamsterColorTypeMaster;
    private Dictionary<int, FacilityMaster> facilityMaster;
    private Dictionary<string, FacilityLevelMaster> facilityLevelMaster;
    private Dictionary<int, FoodMaster> foodMaster;
    private Dictionary<int, UserRankMaster> userRankMaster;
    private Dictionary<int, RarityLotteryMaster> rarityLotteryMaster;

    /// <summary>
    /// Constructor.
    /// </summary>
    public MasterDataDB()
    {
        hamsterMaster = GetHamsterMaster(MasterDefine.Name.HamsterMaster);
        hamsterBugMaster = GetHamsterBugMaster(MasterDefine.Name.HamsterBugMaster);
        hamsterColorTypeMaster = GetHamsterColorTypeMaster(MasterDefine.Name.HamsterColorTypeMaster);
        facilityMaster = GetFacilityMaster(MasterDefine.Name.FacilityMaster);
        facilityLevelMaster = GetFacilityLevelMaster(MasterDefine.Name.FacilityLevelMaster);
        foodMaster = GetFoodMaster(MasterDefine.Name.FoodMaster);
        userRankMaster = GetUserRankMaster(MasterDefine.Name.UserRankMaster);
        rarityLotteryMaster = GetRarityLotteryMaster(MasterDefine.Name.RarityLotteryMaster);
    }

    /// <summary>
    /// マスターをcsvからインポート
    /// </summary>
    /// <param name="masterName">master file name</param>
    /// <returns></returns>
    private List<string[]> ImportMasterCsv(string masterName)
    {
        List<string[]> result = new List<string[]>();
        TextAsset csvFile = Resources.Load<TextAsset>($"Master/{masterName}");

        if(csvFile == null)
        {
            Debug.LogError($"Master Not Found :master name->{masterName}");
        }

        StringReader reader = new StringReader(csvFile.text);

        // 最初の２行がCSV読み込みには不要なので飛ばす
        reader.ReadLine();
        reader.ReadLine();

        while(reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            result.Add(line.Split(','));
        }

        return result;
    }
}
