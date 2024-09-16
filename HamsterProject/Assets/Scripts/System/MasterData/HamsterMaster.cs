public class HamsterMaster
{
    public HamsterMaster(int id, string name, string imagePath, int bugId, float bugFixTime)
    {
        HamsterId = id;
        Name = name; 
        ImagePath= imagePath;
        BugId = bugId;
        BugFixTime = bugFixTime;
    }

    public int HamsterId { get; }
    
    public string Name { get; }
    
    public string ImagePath { get; }
    
    public int BugId { get; }
    
    public float BugFixTime { get; }
}