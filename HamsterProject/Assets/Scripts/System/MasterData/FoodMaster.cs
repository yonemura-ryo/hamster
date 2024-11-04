public class FoodMaster
{
    public FoodMaster(int id, string name, string imagePath, string description, int price, float duration, int effectId)
    {
        FoodId = id;
        Name = name;
        ImagePath = imagePath;
        Description = description;
        Price = price;
        Duration = duration;
        EffectId = effectId;
    }
    public int FoodId { get; }
    public string Name { get; }
    public string ImagePath { get; }
    public string Description { get; }
    public int Price { get; }
    public float Duration { get; }
    public int EffectId { get; }
}