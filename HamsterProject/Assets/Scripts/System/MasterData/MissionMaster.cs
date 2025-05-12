public class MissionMaster
{
    public int MissionId { get; }
    public int MissionTab { get; }
    public int MissionType { get; }
    public int MissionConditionValue { get; }
    public int CompleteValue { get; }
    public int RewardItemId { get; }
    public int RewardItemNum { get; }
    public string MissionText { get; }

    // コンストラクタ
    public MissionMaster(int missionId, int missionTab, int missionType, int missionConditionValue, int completeValue, int rewardItemId, int rewardItemNum, string missionText)
    {
        MissionId = missionId;
        MissionTab = missionTab;
        MissionType = missionType;
        MissionConditionValue = missionConditionValue;
        CompleteValue = completeValue;
        RewardItemId = rewardItemId;
        RewardItemNum = rewardItemNum;
        MissionText = missionText;
    }
}
