using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各シーンのコントローラーのBaseクラス
/// </summary>
public abstract class SceneControllerBase : MonoBehaviour
{
    [SerializeField] private SceneBgmType sceneBgmType;

    private void Start()
    {
        Prepare();
        Initialize();
    }

    /// <summary>
    /// 準備(Asyncなどの待ち処理が必要であれば対応します)
    /// </summary>
    private void Prepare()
    {
        if (sceneBgmType == SceneBgmType.None) return;
        SystemScene.Instance.SoundPlayer.PlayBgm(SceneControllerDefine.SceneBgmTitle[(int)sceneBgmType]);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    protected abstract void Initialize();
}
