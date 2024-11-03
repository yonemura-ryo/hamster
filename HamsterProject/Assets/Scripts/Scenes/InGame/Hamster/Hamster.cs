using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

/// <summary>
/// ハムスターの出現状態
/// </summary>
public enum HamsterStatus
{
    AppearBefore = 0,
    Appear = 1,
    BugFixing = 2,
    BugFixed = 3,
    Finished = 4,
}

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
    private float _requireBugfixTime = 0f;
    private int bugFixReward = 0;
    /// <summary>
    /// ハムスター状態
    /// </summary>
    private HamsterStatus hamsterStatus = HamsterStatus.AppearBefore;

    private DialogContainer _dialogContainer;
    private RectTransform _rectTransform;
    private Action<int,string> _finishFixAction;
    private Action<int, float> _startBugFixAction;
    private int _hamsterIndex = 0;

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
        float requireBugFixTime,
        int bugFixReward,
        Action<int,float> startBugFixAction
        )
    {
        _hamsterIndex = hamsterIndex;
        _finishFixAction = finishFixAction;
        _imagePath = imagePath;
        _fixedHamsterImagePath = normalImagePath;
        _bugId = bugId;
        //this.requireAppearTime = requireAppearTime;
        _requireBugfixTime = requireBugFixTime;
        this.bugFixReward = bugFixReward;
        _startBugFixAction = startBugFixAction;
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
        switch (hamsterStatus)
        {
            // バグ出現前
            case HamsterStatus.AppearBefore:
                return;
            // ハムスター出現中(タップでバグ修正可能)
            case HamsterStatus.Appear:
                MakeBugFixing(false);
                // バグ修正開始
                _startBugFixAction?.Invoke(_hamsterIndex, _requireBugfixTime);
                break;

            // バグ修正完了(タップで獲得)
            case HamsterStatus.BugFixed:
                hamsterStatus = HamsterStatus.Finished;
                // 通常ハムのデータにする
                //finishBugFixDatetime = DateTime.MinValue;
                // 修正が完了しているので通常ハム獲得のち初期化
                // TODO 元のハムスター表示に戻す処理
                hamsterImage.color = Color.white;
                hamsterImage.sprite = Resources.Load<Sprite>(GetHamsterImagePath());
                InitalizeCountDonwText(Color.white, false);
                _finishFixAction(_hamsterIndex, GetFixedHamsterImagePath());
                break;
            // TODO 修正時間短縮ダイアログ出すなど
            case HamsterStatus.BugFixing:
            default:
                break;
        }
    }

    /// <summary>
    /// ハムスターをシルエット表示
    /// </summary>
    public void MakeSilhouette()
    {
        hamsterImage.color = Color.black;
        // カウントダウンテキスト表示
        InitalizeCountDonwText(Color.white);
    }

    /// <summary>
    /// ハムスターのバグ見た目生成
    /// TODO バグの種類が未確定のため、内容によって実装する
    /// </summary>
    public void MakeBug()
    {
        hamsterStatus = HamsterStatus.Appear;
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
    /// バグ修正開始
    /// </summary>
    public void MakeBugFixing(bool bMakeBug=true)
    {
        if (bMakeBug)
        {
            MakeBug();
        }
        hamsterStatus = HamsterStatus.BugFixing;
        // バグ修正開始する
        UpdateFixCountDownText(_requireBugfixTime);
        // カウントダウンテキスト表示
        InitalizeCountDonwText(Color.white);
    }

    /// <summary>
    /// バグ修正完了状態
    /// </summary>
    public void MakeBugFixed()
    {
        hamsterStatus = HamsterStatus.BugFixed;
        UpdateFixCountDownTextByMessage("fixed!!");
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

    /// <summary>
    /// 自由文字表示
    /// </summary>
    /// <param name="message"></param>
    public void UpdateFixCountDownTextByMessage(string message)
    {
        fixCountDownText.text = message;
    }
}
