using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボタンが押されたときのSEタイプ
/// </summary>
public enum ButtonSoundType
{
    None = 0,
    Push = 1,
}

/// <summary>
/// Define Button.
/// </summary>
public class ButtonDefine
{
    /// <summary>
    /// サウンドタイプごとのSE名ゲッター
    /// </summary>
    public static IReadOnlyDictionary<int, string> SoundTypeNames => soundTypeNames;

    private static readonly Dictionary<int, string> soundTypeNames = new Dictionary<int, string>()
    {
        {(int) ButtonSoundType.Push, SoundDefine.Se.ButtonPush },
    };
}
