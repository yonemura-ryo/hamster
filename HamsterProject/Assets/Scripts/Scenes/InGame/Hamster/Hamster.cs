using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

/// <summary>
/// ハムスター単体の処理
/// </summary>
public class Hamster : MonoBehaviour
{
    [SerializeField] Image hamsterImage;
    [SerializeField] TextMeshProUGUI fixCountDownText;
    
    private string _imagePath;
    private string _fixedHamsterImagePath;
    private int _bugId = 0;
    private float requireAppearTime = 0f;
    private float _requireBugfixTime = 0f;
    private int bugFixReward = 0;

    private DateTime finishBugFixDatetime = DateTime.MinValue;
    private DialogContainer _dialogContainer;
    private RectTransform _rectTransform;
    private Action<int,string> _finishFixAction;
    private int _hamsterIndex = 0;
    private bool isAppearBug = false;


    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="hamsterIndex"></param>
    /// <param name="position"></param>
    /// <param name="finishFixAction"></param>
    /// <param name="imagePath"></param>
    /// <param name="requireBugFixTime"></param>
    /// <param name="bugId"></param>
    public void Initialize(
        int hamsterIndex,
        Transform position,
        Action<int,string> finishFixAction,
        string imagePath,
        string normalImagePath,
        int bugId,
        float requireAppearTime,
        float requireBugFixTime,
        int bugFixReward
        )
    {
        _hamsterIndex = hamsterIndex;
        _finishFixAction = finishFixAction;
        _imagePath = imagePath;
        _fixedHamsterImagePath = normalImagePath;
        _bugId = bugId;
        this.requireAppearTime = requireAppearTime;
        _requireBugfixTime = requireBugFixTime;
        this.bugFixReward = bugFixReward;
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.localPosition = position.localPosition;
        hamsterImage.sprite = Resources.Load<Sprite>(GetHamsterImagePath());
    }
    
    /// <summary>
    /// ハム画像パス
    /// </summary>
    /// <returns></returns>
    public string GetHamsterImagePath()
    {
        return "Images/Hamsters/" + _imagePath;
    }
    
    /// <summary>
    /// 修正後のハムスターの画像パス
    /// </summary>
    /// <returns></returns>
    public string GetFixedHamsterImagePath()
    {
        if (_fixedHamsterImagePath == null)
        {
            return "";
        }
        return "Images/Hamsters/" + _fixedHamsterImagePath;
    }

    public int GetHamsterBugId()
    {
        return _bugId;
    }

    /// <summary>
    /// バグ修正報酬
    /// </summary>
    /// <returns></returns>
    public int GetBugFixReward()
    {
        return bugFixReward;
    }
    
    /// <summary>
    /// ハムスタータップ時処理
    /// </summary>
    public void OnClickHamster()
    {
        // バグ出現前
        if (!isAppearBug)
        {
            return;
        }

        // TODO 修正時間の操作系はManager側に移す（予定）

        //if (_bugId <= 0)
        //{
        //    // TODO 通常ハムタップ時の動作検討
        //    // 2回タップしたら消える
        //    _disappearCount++;
        //    if (_disappearCount >= 2)
        //    {
        //        _disappearAction?.Invoke(_hamsterIndex);
        //    }
        //    return;
        //}

        // バグ修正開始前
        if (finishBugFixDatetime == DateTime.MinValue)
        {
            
            // 終了日時を設定
            TimeSpan timeSpan = TimeSpan.FromSeconds(_requireBugfixTime);
            finishBugFixDatetime = DateTime.Now.Add(timeSpan);
            // バグ修正開始する
            UpdateFixCountDownText(_requireBugfixTime);
            // カウントダウンテキスト表示
            InitalizeCountDonwText(Color.white);
                
            DOTween.To(() => _requireBugfixTime, x => _requireBugfixTime = x, 0f, _requireBugfixTime)
                .SetEase(Ease.Linear)    
                .OnUpdate(() =>
                {
                    // 現在のカウントダウン秒数から、日時を生成して表示
                    UpdateFixCountDownText(_requireBugfixTime);
                })
                .OnComplete(() =>
                {
                    fixCountDownText.text = "fixed!";
                    Debug.Log("Complete bug fix");
                });
            return;
        }

        // 修正完了
        if (finishBugFixDatetime < DateTime.Now)
        {
            // 通常ハムのデータにする
            finishBugFixDatetime = DateTime.MinValue;
            // 修正が完了しているので通常ハム獲得のち初期化
            // TODO 元のハムスター表示に戻す処理
            hamsterImage.color = Color.white;
            hamsterImage.sprite = Resources.Load<Sprite>(GetHamsterImagePath());
            InitalizeCountDonwText(Color.white,false);
            _finishFixAction(_hamsterIndex, GetFixedHamsterImagePath());
            return;
        }
        
        // ここにくるのは修正中のため、短縮するかダイアログ出す
        // TODO 短縮ダイアログ出す
        return;
    }

    /// <summary>
    /// ハムスターをシルエット表示
    /// </summary>
    public void MakeSilhouette()
    {
        hamsterImage.color = Color.black;
        // カウントダウンテキスト表示
        //fixCountDownText.gameObject.SetActive(true);
        //fixCountDownText.enabled = true;
        InitalizeCountDonwText(Color.white);
        //DOTween.To(() =>  requireAppearTime, x => requireAppearTime = x, 0f, requireAppearTime)
        //        .SetEase(Ease.Linear)
        //        .OnUpdate(() =>
        //        {
        //            // 現在のカウントダウン秒数から、日時を生成して表示
        //            UpdateFixCountDownText();
        //        });
    }

    /// <summary>
    /// ハムスターのバグ見た目生成
    /// TODO バグの種類が未確定のため、内容によって実装する
    /// </summary>
    public void MakeBug()
    {
        isAppearBug = true;
        InitalizeCountDonwText(Color.white, false);
        hamsterImage.color = Color.white;
        switch (_bugId)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                // TODO 仮　色を変える
                hamsterImage.color = Color.red;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// カウントダウンテキストの初期化
    /// </summary>
    /// <param name="textColor"></param>
    /// <param name="isActive"></param>
    private void InitalizeCountDonwText(Color textColor, bool isActive = true)
    {
        fixCountDownText.gameObject.SetActive(isActive);
        fixCountDownText.enabled = isActive;
        fixCountDownText.color = textColor;
    }
    
    /// <summary>
    /// 残り時間表示の更新
    /// </summary>
    public void UpdateFixCountDownText(float countDownTime)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(countDownTime);
        fixCountDownText.text = timeSpan.ToString("mm':'ss");
    }
}
