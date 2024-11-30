public class FoodMaster
{
    public FoodMaster(int id, string name, string imagePath, int fixTimeRate, int lotteryId, string description, int price, float duration, int effectId)
    {
        FoodId = id;
        Name = name;
        ImagePath = imagePath;
        FixTimeRate = fixTimeRate;
        LotteryId = lotteryId;
        Description = description;
        Price = price;
        Duration = duration;
        EffectId = effectId;
    }
    public int FoodId { get; }
    public string Name { get; }
    public string ImagePath { get; }
    public int FixTimeRate { get; }
    public int LotteryId { get; }
    public string Description { get; }
    public int Price { get; }
    public float Duration { get; }
    public int EffectId { get; }
}