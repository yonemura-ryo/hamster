
using System.Collections.Generic;
/// <summary>
/// SceneController周りのDefine
/// </summary>
public class SceneControllerDefine
{

    public static readonly Dictionary<int, string> SceneBgmTitle = new Dictionary<int, string>()
    {
        { (int)SceneBgmType.ForwardTommorow, SoundDefine.Bgm.ForwardTommorow }
    };
}

/// <summary>
/// BGMの設定Enum
/// </summary>
public enum SceneBgmType
{
    None = 0,
    ForwardTommorow = 1,
}
