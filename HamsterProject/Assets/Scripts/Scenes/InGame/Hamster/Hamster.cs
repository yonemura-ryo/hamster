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
    private float _requireBugfixTime = 0f;
    private DateTime finishBugFixDatetime = DateTime.MinValue;
    private DialogContainer _dialogContainer;
    private RectTransform _rectTransform;
    private Action<int,string> _finishFixAction;
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
    public void Initialize(int hamsterIndex, Transform position, Action<int,string> finishFixAction, string imagePath, float requireBugFixTime, int bugId)
    {
        _hamsterIndex = hamsterIndex;
        _finishFixAction = finishFixAction;
        _imagePath = imagePath;
        _fixedHamsterImagePath = imagePath;
        _requireBugfixTime = requireBugFixTime;
        _bugId = bugId;
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.localPosition = position.localPosition;
        
        hamsterImage.sprite = Resources.Load<Sprite>(GetHamsterImagePath());
        MakeBug();
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
    
    /// <summary>
    /// ハムスタータップ時処理
    /// </summary>
    public void OnClickHamster()
    {
        // TODO 修正時間の操作系はManager側に移す（予定）
        if (_bugId <= 0)
        {
            // バグじゃなければ一旦何もしない
            return;
        }

        // バグ修正開始前
        if (finishBugFixDatetime == DateTime.MinValue)
        {
            // 終了日時を設定
            TimeSpan timeSpan = TimeSpan.FromSeconds(_requireBugfixTime);
            finishBugFixDatetime = DateTime.Now.Add(timeSpan);
            // バグ修正開始する
            UpdateFixCountDownText();
            // カウントダウンテキスト表示
            fixCountDownText.gameObject.SetActive(true);
            fixCountDownText.enabled = true;
                
            DOTween.To(() => _requireBugfixTime, x => _requireBugfixTime = x, 0f, _requireBugfixTime)
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
            fixCountDownText.gameObject.SetActive(false);
            fixCountDownText.enabled = false;
            _finishFixAction(_hamsterIndex, GetFixedHamsterImagePath());
            return;
        }
        
        // ここにくるのは修正中のため、短縮するかダイアログ出す
        // TODO 短縮ダイアログ出す
        return;
    }
    
    /// <summary>
    /// ハムスターのバグ見た目生成
    /// TODO バグの種類が未確定のため、内容によって実装する
    /// </summary>
    private void MakeBug()
    {
        switch (_bugId)
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
    
    /// <summary>
    /// 残り時間表示の更新
    /// </summary>
    private void UpdateFixCountDownText()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(_requireBugfixTime);
        fixCountDownText.text = timeSpan.ToString("mm':'ss");
    }
}
