using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;

public class HamsterPresenter : MonoBehaviour
{
    private HamsterModel hamsterModel;
    [SerializeField] Image hamsterImage;
    [SerializeField] TextMeshProUGUI fixCountDownText;
    private float bugFixCountDown;

    private Action finishFixAction;

    /// <summary>
    /// Initialize.
    /// </summary>
    /// <param name="hamsterModel"></param>
    public void Initialize(HamsterModel hamsterModel, Action finishFixAction)
    {
        this.finishFixAction = finishFixAction;
        this.hamsterModel = hamsterModel;
        // ハム画像変更
        hamsterImage.sprite = Resources.Load<Sprite>(this.hamsterModel.GetHamsterImagePath());
        MakeBug();
    }

    public void OnClickHamster()
    {
        var fixType = hamsterModel.OnClickFixBug();
        Debug.Log("修正タイプ" + fixType);
        switch (fixType)
        {
            // 修正開始
            case HamsterDefine.ClickFunctionType.StartFix:
                bugFixCountDown = hamsterModel.GetBugFixTime();
                UpdateFixCountDownText();
                // カウントダウンテキスト表示
                fixCountDownText.gameObject.SetActive(true);
                fixCountDownText.enabled = true;
                
                DOTween.To(() => bugFixCountDown, x => bugFixCountDown = x, 0f, hamsterModel.GetBugFixTime())
                    .SetEase(Ease.Linear)    
                    .OnUpdate(() =>
                    {
                        // 現在のカウントダウン秒数から、日時を生成して表示
                        UpdateFixCountDownText();
                    })
                    .OnComplete(() =>
                    {
                        fixCountDownText.text = "fixed!";
                        Debug.Log("Complete bug fix");
                    });
                break;
            case HamsterDefine.ClickFunctionType.FinishFix:
                // TODO 仮
                hamsterImage.color = Color.white;
                hamsterImage.sprite = Resources.Load<Sprite>(this.hamsterModel.GetHamsterImagePath());
                fixCountDownText.gameObject.SetActive(false);
                fixCountDownText.enabled = false;
                finishFixAction?.Invoke();
                break;
            default:
                // いったんなにもしない
                break;
        }
    }

    /// <summary>
    /// TODO 仮　ハムスターのバグ見た目生成
    /// </summary>
    private void MakeBug()
    {
        switch (hamsterModel.GetBugID())
        {
            case 0:
                break;
            case 1:
                // TODO 仮　色を変える
                hamsterImage.color = Color.red;
                break;
            default:
                break;
        }
    }

    private void UpdateFixCountDownText()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(bugFixCountDown);
        fixCountDownText.text = timeSpan.ToString("mm':'ss");
    }
}