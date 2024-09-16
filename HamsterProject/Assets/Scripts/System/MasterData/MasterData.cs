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

    private Dictionary<int, HamsterMaster> hamsterMaster;

    /// <summary>
    /// Constructor.
    /// </summary>
    public MasterDataDB()
    {
        hamsterMaster = GetHamsterMaster(MasterDefine.Name.HamsterMaster);
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
