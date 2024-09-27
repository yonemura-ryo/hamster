
using System.Collections.Generic;
/// <summary>
/// SceneController�����Define
/// </summary>
public class SceneControllerDefine
{

    public static readonly Dictionary<int, string> SceneBgmTitle = new Dictionary<int, string>()
    {
        { (int)SceneBgmType.Bgm01, SoundDefine.Bgm.Bgm01 }
    };
}

/// <summary>
/// BGM�̐ݒ�Enum
/// </summary>
public enum SceneBgmType
{
    None = 0,
    Bgm01 = 1,
}
