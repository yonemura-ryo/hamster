public class FacilityMaster
{
    public FacilityMaster(int id, string name, int levelType)
    {
        FacilityId = id;
        Name = name;
        LevelType = levelType;
    }
    public int FacilityId { get; }
    public string Name { get; }
    public int LevelType { get; }
}