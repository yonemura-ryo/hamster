using System;
using System.Collections.Generic;
using System.Linq;

public class MissionManager
{
    /// <summary>
    /// ミッションマスタ
    /// </summary>
    private IReadOnlyDictionary<int, MissionMaster> missionMasters = null;
    /// <summary>
    /// [セーブデータ]ミッション進行状態データ
    /// </summary>
    private MissionProgressListData missionProgressListData = null;
    public MissionProgressListData missionProgressListDataReadOnly => missionProgressListData;

    public MissionManager()
    {
        // マスタ読み込み
        missionMasters = MasterData.DB.MissionMaster;
        // セーブデータ読み込み
        missionProgressListData = LocalPrefs.Load<MissionProgressListData>(SaveData.Key.MissionProgressListData);
        if (missionProgressListData == null)
        {
            missionProgressListData = new MissionProgressListData();
            missionProgressListData.missionProgressDictionary = new SerializableDictionary<int, MissionProgressData>();
            LocalPrefs.Save(SaveData.Key.MissionProgressListData, missionProgressListData);
        }
    }

    /// <summary>
    /// ミッション受取
    /// </summary>
    /// <param name="missionId"></param>
    public void receiveMission(int missionId, int itemId, int itemNum, Action<int,int> AcquireFood)
    {
        // TODO パターン別のモーダル表示
        if (missionProgressListData.missionProgressDictionary.ContainsKey(missionId))
        {
            MissionProgressData missionProgressData = missionProgressListData.missionProgressDictionary[missionId];
            if (missionProgressData.isReceived)
            {
                // すでに受取済み
            }
            else if (!missionProgressData.isCompleted)
            {
                // 完了してない
            }
            else
            {
                // TODO アイテム付与
                AcquireFood(itemId, itemNum);
                // 受取済み
                missionProgressData.isReceived = true;
                missionProgressListData.missionProgressDictionary[missionId] = missionProgressData;
                LocalPrefs.Save(SaveData.Key.MissionProgressListData, missionProgressListData);
            }
        }
    }

    /// <summary>
    /// ミッション進行
    /// </summary>
    /// <param name="missionId"></param>
    /// <param name="addValue"></param>
    /// <returns>達成フラグ</returns>
    public bool progressMission(int missionId, int addValue, bool isAbsoluteValue=false)
    {
        MissionMaster missionMaster = missionMasters[missionId];
        MissionProgressData missionProgressData = new MissionProgressData();
        if (missionProgressListData.missionProgressDictionary.ContainsKey(missionId))
        {
            // データあり
            missionProgressData = missionProgressListData.missionProgressDictionary[missionId];
            if (missionProgressData.isCompleted)
            {
                // すでに達成済み
                return false;
            }
            if (isAbsoluteValue)
            {
                missionProgressData.progressValue = addValue;
            }
            else
            {
                missionProgressData.progressValue += addValue;
            }
        }
        else
        {
            // 初回
            missionProgressData.missionId = missionId;
            missionProgressData.progressValue = addValue;
            missionProgressData.isCompleted = false;
            missionProgressData.isReceived = false;
        }
        // 達成チェック
        if(missionProgressData.progressValue >= missionMaster.CompleteValue)
        {
            // 達成している
            missionProgressData.isCompleted = true;
            // 進行値は達成値に設定する
            missionProgressData.progressValue = missionMaster.CompleteValue;
        }
        // 保存
        missionProgressListData.missionProgressDictionary[missionId] = missionProgressData;
        LocalPrefs.Save(SaveData.Key.MissionProgressListData, missionProgressListData);
        return missionProgressData.isCompleted;
    }

    /// <summary>
    /// ハムスター修正完了時のミッション進行
    /// </summary>
    /// <param name="hamsterId"></param>
    public void missionProgressFixBugster(int hamsterId)
    {
        // 累計修正回数
        //MissionDefine.Type.BUGSTER_TOTAL_FIX_COUNT;
        var totalFixMissions = missionMasters
            .Where(x => x.Value.MissionType == MissionDefine.Type.BUGSTER_TOTAL_FIX_COUNT)
        ;
        foreach(KeyValuePair<int, MissionMaster> kvp in totalFixMissions)
        {
            // 捕獲数をカウントする
            progressMission(kvp.Key, 1);
        }
        // TODO そのほかのハムスター修正関連ミッション
        // XXXXバグスターをn匹修正した
        // バグスターをn種類修正した
        // 累計修正時間がn時間を突破した
    }

    /// <summary>
    /// 施設強化ミッション
    /// </summary>
    /// <param name="facilityId"></param>
    /// <param name="strengthCount"></param>
    public void missionProgressFacilityStrength(int facilityId, int strengthCount=1)
    {
        // 施設「XXX」をn回強化した
        var targetMissions = missionMasters
                .Where(x => x.Value.MissionType == MissionDefine.Type.TARGET_FACILITY_STRENGTH_COUNT)
                .Where(x => x.Value.MissionConditionValue == facilityId)
            ;
        foreach (KeyValuePair<int, MissionMaster> kvp in targetMissions)
        {
            // 施設強化数をカウントする
            progressMission(kvp.Key, strengthCount);
        }
    }

    /// <summary>
    /// ユーザーランク上昇ミッション
    /// </summary>
    /// <param name="newUserRank"></param>
    public void missionProgressUserRank(int newUserRank)
    {
        // ユーザーランクをnにした
        var targetMissions = missionMasters
            .Where(x => x.Value.MissionType == MissionDefine.Type.USER_RANK)
        ;
        foreach (KeyValuePair<int, MissionMaster> kvp in targetMissions)
        {
            // ユーザーランク上昇分をカウントする
            progressMission(kvp.Key, newUserRank, true);
        }
    }

    /// <summary>
    /// コイン取得ミッション
    /// </summary>
    /// <param name="addCoinCount"></param>
    public void missionProgressGetCoin(int addCoinCount)
    {
        //累計nコイン集めた
        if (addCoinCount > 0)
        {
            var targetMissions = missionMasters
                .Where(x => x.Value.MissionType == MissionDefine.Type.COIN_TOTAL_GET_COUNT)
            ;
            foreach (KeyValuePair<int, MissionMaster> kvp in targetMissions)
            {
                // 取得コイン数をカウントする
                progressMission(kvp.Key, addCoinCount);
            }
        }
    }

    /// <summary>
    /// [デバッグ用]ミッションのリセット
    /// </summary>
    /// <param name="missionId"></param>
    public void resetMission(int missionId)
    {
        MissionProgressData missionProgressData = new MissionProgressData();
        missionProgressData.missionId = missionId;
        missionProgressData.isCompleted = false;
        missionProgressData.isReceived = false;
        missionProgressData.progressValue = 0;
        // 保存
        missionProgressListData.missionProgressDictionary[missionId] = missionProgressData;
        LocalPrefs.Save(SaveData.Key.MissionProgressListData, missionProgressListData);
    }

    /// <summary>
    /// [デバッグ用]ミッションのクリア
    /// </summary>
    /// <param name="missionId"></param>
    public void completeMission(int missionId)
    {
        MissionMaster missionMaster = missionMasters[missionId];
        MissionProgressData missionProgressData = new MissionProgressData();
        missionProgressData.missionId = missionId;
        missionProgressData.isCompleted = true;
        missionProgressData.isReceived = false;
        missionProgressData.progressValue = missionMaster.CompleteValue;
        // 保存
        missionProgressListData.missionProgressDictionary[missionId] = missionProgressData;
        LocalPrefs.Save(SaveData.Key.MissionProgressListData, missionProgressListData);
    }

    /// <summary>
    /// ミッションID全取得
    /// </summary>
    /// <returns></returns>
    public List<int> getAllMissionIds()
    {
        List<int> missionIds = new List<int>();
        foreach((int missionId, MissionMaster missionMaster) in missionMasters){
            missionIds.Add(missionId);
        }
        return missionIds;
    }
}
