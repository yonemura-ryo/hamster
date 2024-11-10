public class FacilityMaster
{
    public FacilityMaster(int id, string name, string imagePath, string description, int levelType)
    {
        FacilityId = id;
        Name = name;
        ImagePath = imagePath;
        Description = description;
        LevelType = levelType;
    }
    public int FacilityId { get; }
    public string Name { get; }
    public string ImagePath { get; }
    public string Description { get; }
    public int LevelType { get; }
}