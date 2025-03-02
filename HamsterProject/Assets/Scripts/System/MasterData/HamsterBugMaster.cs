using System;
public class HamsterBugMaster
{
    public HamsterBugMaster(int bugId, int rare)
    {
        BugId = bugId;
        Rare = rare;
    }

    public int BugId { get; }
    public int Rare { get; }
}
