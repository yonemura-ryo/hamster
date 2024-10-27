public class UserRankMaster
{
    public UserRankMaster(int rank, int exp, int functionReleaseId)
    {
        Rank = rank;
        Exp = exp;
        FunctionReleaseId = functionReleaseId;
    }

    public int Rank { get; }
    public int Exp { get; }
    public int FunctionReleaseId { get; }
}
