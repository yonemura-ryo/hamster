public class FacilityLevelMaster
{
    public FacilityLevelMaster(int levelType, int level, int needCoin)
    {
        LevelType = levelType;
        Level = level;
        NeedCoin = needCoin;
    }
    
    public int LevelType { get; }
    public int Level { get; }
    public int NeedCoin { get; }
}