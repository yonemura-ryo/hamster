public class FoodMaster
{
    public FoodMaster(int id, string name, int price, float duration)
    {
        FoodId = id;
        Name = name;
        Price = price;
        Duration = duration;
    }
    public int FoodId { get; }
    public string Name { get; }
    public int Price { get; }
    public float Duration { get; }
}