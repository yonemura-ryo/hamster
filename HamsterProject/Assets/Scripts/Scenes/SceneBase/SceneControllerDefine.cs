
using System.Collections.Generic;
/// <summary>
/// SceneControllerü‚è‚ÌDefine
/// </summary>
public class SceneControllerDefine
{

    public static readonly Dictionary<int, string> SceneBgmTitle = new Dictionary<int, string>()
    {
        { (int)SceneBgmType.Bgm01, SoundDefine.Bgm.Bgm01 }
    };
}

/// <summary>
/// BGM‚Ìİ’èEnum
/// </summary>
public enum SceneBgmType
{
    None = 0,
    Bgm01 = 1,
}
