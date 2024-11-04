using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UniRx;
using System;

/// <summary>
/// InGame Scene Presenter.
/// </summary>
public class InGamePresenter : MonoBehaviour
{
    /// <summary> Model. </summary>
    private InGameModel inGameModel;
    private IDialogContainer dialogContainer;

    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI rankText;

    [SerializeField] private CustomButton MenuButton = null;
    [SerializeField] private CustomButton ShopButton = null;

    /// <summary>
    /// Initialize.
    /// </summary>
    /// <param name="inGameModel"></param>
    public void Initialize(
        InGameModel inGameModel,
        IDialogContainer dialogContainer,
        ISoundPlayer soundPlayer,
        ISceneTransitioner sceneTransitioner,
        UserCommonData userCommonData,
        Action<int> acquireCoin,
        Action<int, int> acquireFood
        )
    {
        this.inGameModel = inGameModel;
        this.dialogContainer = dialogContainer;
        UpdateCoinText(userCommonData.coinCount);
        UpdateRankText(userCommonData.userRank);

        MenuButton.OnClickAsObservable().Subscribe(_ =>
        {
            MenuDialog menuDialog = dialogContainer.Show<MenuDialog>(null);
            menuDialog.Initialize(dialogContainer, soundPlayer, sceneTransitioner);
        }).AddTo(this);

        // ショップ
        ShopButton.OnClickAsObservable().Subscribe(_ =>
        {
            ShopDialog shopDialog = dialogContainer.Show<ShopDialog>(null);
            shopDialog.Initialize(dialogContainer, soundPlayer, sceneTransitioner, userCommonData,
                (foodId, count, price) =>
                {
                    acquireCoin?.Invoke(-price);
                    acquireFood?.Invoke(foodId, count);
                }
                );
        }).AddTo(this);
    }

    /// <summary>
    /// 所持コイン表示の更新
    /// </summary>
    /// <param name="coinCount"></param>
    public void UpdateCoinText(int coinCount)
    {
        coinText.text = coinCount.ToString();
    }

    public void UpdateRankText(int rank)
    {
        rankText.text = rank.ToString();
    }
}