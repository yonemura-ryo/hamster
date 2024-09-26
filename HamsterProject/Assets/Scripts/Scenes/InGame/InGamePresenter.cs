using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

/// <summary>
/// InGame Scene Presenter.
/// </summary>
public class InGamePresenter : MonoBehaviour
{
    /// <summary> Model. </summary>
    private InGameModel inGameModel;
    
    [SerializeField] private TextMeshProUGUI coinText;

    /// <summary>
    /// Initialize.
    /// </summary>
    /// <param name="inGameModel"></param>
    public void Initialize(InGameModel inGameModel, int coinCount=0)
    {
        this.inGameModel = inGameModel;
        UpdateCoinText(coinCount);
    }

    /// <summary>
    /// 所持コイン表示の更新
    /// </summary>
    /// <param name="coinCount"></param>
    public void UpdateCoinText(int coinCount)
    {
        coinText.text = coinCount.ToString();
    }
}