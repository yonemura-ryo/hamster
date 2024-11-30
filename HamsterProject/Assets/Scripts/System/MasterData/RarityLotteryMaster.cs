public class RarityLotteryMaster
{
    public RarityLotteryMaster(
        int id,
        int rare1,
        int rare2,
        int rare3,
        int rare4,
        int rare5
        )
    {
        LotteryId = id;
        Rare1 = rare1;
        Rare2 = rare2;
        Rare3 = rare3;
        Rare4 = rare4;
        Rare5 = rare5;
    }
    public int LotteryId { get; }
    public int Rare1 { get; }
    public int Rare2 { get; }
    public int Rare3 { get; }
    public int Rare4 { get; }
    public int Rare5 { get; }
}
