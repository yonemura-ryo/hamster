using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UniRx;

/// <summary>
/// InGame Scene Presenter.
/// </summary>
public class InGamePresenter : MonoBehaviour
{
    /// <summary> Model. </summary>
    private InGameModel inGameModel;
    private IDialogContainer dialogContainer;
    
    [SerializeField] private TextMeshProUGUI coinText;

    [SerializeField] private CustomButton MenuButton = null;

    /// <summary>
    /// Initialize.
    /// </summary>
    /// <param name="inGameModel"></param>
    public void Initialize(InGameModel inGameModel, IDialogContainer dialogContainer, ISoundPlayer soundPlayer, ISceneTransitioner sceneTransitioner, int coinCount=0)
    {
        this.inGameModel = inGameModel;
        this.dialogContainer = dialogContainer;
        UpdateCoinText(coinCount);

        MenuButton.OnClickAsObservable().Subscribe(_ =>
        {
            MenuDialog menuDialog = dialogContainer.Show<MenuDialog>(null);
            menuDialog.Initialize(dialogContainer, soundPlayer, sceneTransitioner);
        });
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