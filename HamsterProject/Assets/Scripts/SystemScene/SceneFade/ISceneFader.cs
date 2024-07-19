using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scene Fader Interface.
/// </summary>
public interface ISceneFader
{
    /// <summary>
    /// Fade In.
    /// </summary>
    /// <param name="onComplete">fire when complete fade in.</param>
    void FadeIn(Action onComplete = null);

    /// <summary>
    /// Fade Out.
    /// </summary>
    /// <param name="onComplete">fire when complete fade out.</param>
    void FadeOut(Action onComplete = null);
}
