public class UserRankMaster
{
    public UserRankMaster(int rank, int exp, int functionReleaseId, int intervalTime)
    {
        Rank = rank;
        Exp = exp;
        FunctionReleaseId = functionReleaseId;
        IntervalTime = intervalTime;
    }

    public int Rank { get; }
    public int Exp { get; }
    public int FunctionReleaseId { get; }
    public int IntervalTime { get; }
}
