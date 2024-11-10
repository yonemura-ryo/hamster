public class FacilityLevelMaster
{
    public FacilityLevelMaster(int levelType, int level, int needCoin, string replaceText)
    {
        LevelType = levelType;
        Level = level;
        NeedCoin = needCoin;
        ReplaceText = replaceText;
    }
    
    public int LevelType { get; }
    public int Level { get; }
    public int NeedCoin { get; }
    public string ReplaceText { get; }
}