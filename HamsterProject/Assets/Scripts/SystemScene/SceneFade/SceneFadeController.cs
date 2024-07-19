using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controll Scene Fade.
/// </summary>
public class SceneFadeController : MonoBehaviour, ISceneFader
{
    [SerializeField] private SceneFadePresenter presenter;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        presenter.Initialize();
    }

    /// <summary>
    /// FadeInアニメーションの開始
    /// </summary>
    /// <param name="onComplete"></param>
    public void FadeIn(Action onComplete = null)
    {
        presenter.StartFadeInAnimation(onComplete);
    }

    /// <summary>
    /// FadeOutアニメーションの開始
    /// </summary>
    /// <param name="onComplete"></param>
    public void FadeOut(Action onComplete = null)
    {
        presenter.StartFadeOutAnimation(onComplete);
    }
}
