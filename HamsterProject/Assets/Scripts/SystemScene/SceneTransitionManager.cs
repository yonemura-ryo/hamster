using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager Class Scene Transition.
/// </summary>
public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] SceneTransitioner sceneTransitioner = null;

    /// <summary>
    /// シーン遷移引き渡し用パラメータ
    /// </summary>
    public object SceneParam => sceneTransitioner.SceneParam;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="sceneTransitionCallback"></param>
    public void Initialize(Action<bool> sceneTransitionCallback)
    {
        sceneTransitioner.Initialize(sceneTransitionCallback);
    }

    /// <summary>
    /// インターフェース取得用
    /// </summary>
    /// <returns></returns>
    public ISceneTransitioner GetSceneTransitioner()
    {
        return sceneTransitioner;
    }
}
