using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

/// <summary>
/// Splash Scene Presenter.
/// </summary>
public class SplashPresenter : MonoBehaviour
{
    [SerializeField] private Image backGroundImage;
    [SerializeField] private Image splashImage;
    [SerializeField] private TextMeshProUGUI splashText;

    /// <summary> Model. </summary>
    private SplashModel splashModel;

    /// <summary> アニメーション秒数 </summary>
    const float AnimationTime = 1.0f;

    /// <summary>
    /// Initialize.
    /// </summary>
    /// <param name="splashModel"></param>
    public void Initialize(SplashModel splashModel)
    {
        this.splashModel = splashModel;

        splashImage.DOFade(1.0f, AnimationTime);
        splashText.DOFade(1.0f, AnimationTime).OnComplete(() =>
        {
            this.splashModel.NextScene();
        });
    }
}
