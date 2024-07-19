
using System.Collections.Generic;
/// <summary>
/// SceneControlleré¸ÇËÇÃDefine
/// </summary>
public class SceneControllerDefine
{

    public static readonly Dictionary<int, string> SceneBgmTitle = new Dictionary<int, string>()
    {
        { (int)SceneBgmType.ForwardTommorow, SoundDefine.Bgm.ForwardTommorow }
    };
}

/// <summary>
/// BGMÇÃê›íËEnum
/// </summary>
public enum SceneBgmType
{
    None = 0,
    ForwardTommorow = 1,
}
