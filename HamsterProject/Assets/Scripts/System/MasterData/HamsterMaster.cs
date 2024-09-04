using MasterMemory;
using MessagePack;

[MemoryTable("HamsterMaster"), MessagePackObject(true)]
public class HamsterMaster
{
    public HamsterMaster(int Id, string Name, string imagePath)
    {
        hamsterID = Id;
    }

    [PrimaryKey]
    public int hamsterID { get; set; }

    public int HamsterID
    {
        get { return hamsterID; }
    }
    
    private string name;

    public string Name {
        get { return name; }
    }

    private string imagePath;

    public string ImagePath
    {
        get { return imagePath; }
    }

    private int bugID;

    public int BugID
    {
        get { return bugID; }
    }

    private float bugFixTime;

    public float BugFixTime
    {
        get { return bugFixTime; }
    }
}