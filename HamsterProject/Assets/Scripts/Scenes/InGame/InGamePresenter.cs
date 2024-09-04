using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

// InGame画面のＵＩをつかさどる

/// <summary>
/// InGame Scene Presenter.
/// </summary>
public class InGamePresenter : MonoBehaviour
{
    // Header
    // Footer 
    // もろもろ

    /// <summary> Model. </summary>
    private InGameModel inGameModel;

    /// <summary>
    /// Initialize.
    /// </summary>
    /// <param name="inGameModel"></param>
    public void Initialize(InGameModel inGameModel)
    {
        this.inGameModel = inGameModel;
    }
}