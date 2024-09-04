using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

// InGame‰æ–Ê‚Ì‚t‚h‚ð‚Â‚©‚³‚Ç‚é

/// <summary>
/// InGame Scene Presenter.
/// </summary>
public class InGamePresenter : MonoBehaviour
{
    // Header
    // Footer 
    // ‚à‚ë‚à‚ë

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