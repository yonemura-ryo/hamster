public class HamsterMaster
{
    public HamsterMaster(
        int id,
        int rare,
        string name,
        string imagePath,
        string normalImagePath,
        int bugId,
        float bugAppearTime,
        float bugFixTime,
        int bugFixReward,
        int appearUserRank,
        int colorTypeId
        )
    {
        HamsterId = id;
        Rare = rare;
        Name = name; 
        ImagePath= imagePath;
        NormalImagePath = normalImagePath;
        BugId = bugId;
        BugAppearTime = bugAppearTime;
        BugFixTime = bugFixTime;
        BugFixReward = bugFixReward;
        AppearUserRank = appearUserRank;
        ColorTypeId = colorTypeId;
    }

    public int HamsterId { get; }

    public int Rare { get; }
    
    public string Name { get; }
    
    public string ImagePath { get; }

    public string NormalImagePath { get; }
    
    public int BugId { get; }

    public float BugAppearTime { get; }
    
    public float BugFixTime { get; }

    public int BugFixReward { get; }

    public int AppearUserRank { get; }

    public int ColorTypeId { get; }
}