using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Scene Fade Presenter.
/// </summary>
public class SceneFadePresenter : MonoBehaviour
{
    [SerializeField] private Image upperRightImage;
    [SerializeField] private Image upperLeftImage;
    [SerializeField] private Image footerRightImage;
    [SerializeField] private Image footerLeftImage;

    [SerializeField] private float animationTime = 1.5f;
    [SerializeField] private float fadeTime = 0.3f;

    #region 座標周り
    private const float InPosX = 280f;
    private const float InPosY = 150f;
    private const float OutPosX = 800f;
    private const float OutPosY = 450f;
    #endregion

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        upperRightImage.DOFade(0f, 0f);
        upperLeftImage.DOFade(0f, 0f);
        footerRightImage.DOFade(0f, 0f);
        footerLeftImage.DOFade(0f, 0f);

        upperLeftImage.rectTransform.localPosition = new Vector2(-OutPosX, OutPosY);
        upperRightImage.rectTransform.localPosition = new Vector2(OutPosX, OutPosY);
        footerLeftImage.rectTransform.localPosition = new Vector2(-OutPosX, -OutPosY);
        footerRightImage.rectTransform.localPosition = new Vector2(OutPosX, -OutPosY);
    }

    /// <summary>
    /// フェードアウトアニメーション開始
    /// </summary>
    /// <param name="onComplete"></param>
    public void StartFadeOutAnimation(Action onComplete = null)
    {
        upperRightImage.DOFade(0f, fadeTime);
        upperLeftImage.DOFade(0f, fadeTime);
        footerRightImage.DOFade(0f, fadeTime);
        footerLeftImage.DOFade(0f, fadeTime);

        upperLeftImage.rectTransform.DOLocalMove(new Vector2(-OutPosX, OutPosY), animationTime);
        upperRightImage.rectTransform.DOLocalMove(new Vector2(OutPosX, OutPosY), animationTime);
        footerLeftImage.rectTransform.DOLocalMove(new Vector2(-OutPosX, -OutPosY), animationTime);
        footerRightImage.rectTransform.DOLocalMove(new Vector2(OutPosX, -OutPosY), animationTime)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
            });
    }

    /// <summary>
    /// フェードインアニメーション開始
    /// </summary>
    /// <param name="onComplete"></param>
    public void StartFadeInAnimation(Action onComplete = null)
    {
        upperRightImage.DOFade(1f, 1 - fadeTime);
        upperLeftImage.DOFade(1f, 1 - fadeTime);
        footerRightImage.DOFade(1f, 1 - fadeTime);
        footerLeftImage.DOFade(1f, 1 - fadeTime);

        upperLeftImage.rectTransform.DOLocalMove(new Vector2(-InPosX, InPosY), animationTime);
        upperRightImage.rectTransform.DOLocalMove(new Vector2(InPosX, InPosY), animationTime);
        footerLeftImage.rectTransform.DOLocalMove(new Vector2(-InPosX, -InPosY), animationTime);
        footerRightImage.rectTransform.DOLocalMove(new Vector2(InPosX, -InPosY), animationTime)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
            });
    }
}
