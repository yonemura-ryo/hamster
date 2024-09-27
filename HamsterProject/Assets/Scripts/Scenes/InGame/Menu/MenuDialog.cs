using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class MenuDialog : DialogBase
{
    [SerializeField] CustomButton BookButton  = null; // 図鑑
    [SerializeField] CustomButton ShopButton  = null; // ショップ
    [SerializeField] CustomButton EnhanceButton  = null; // 強化
    [SerializeField] CustomButton MissionButton  = null; // ミッション
    [SerializeField] CustomButton HowToButton  = null; // 遊び方
    [SerializeField] CustomButton SettingsButton  = null; // 設定

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        BookButton.OnClickAsObservable().Subscribe(_ =>
        {

        }).AddTo(this);

        ShopButton.OnClickAsObservable().Subscribe(_ =>
        {

        }).AddTo(this);

        EnhanceButton.OnClickAsObservable().Subscribe(_ =>
        {

        }).AddTo(this);

        MissionButton.OnClickAsObservable().Subscribe(_ =>
        {

        }).AddTo(this);

        HowToButton.OnClickAsObservable().Subscribe(_ =>
        {

        }).AddTo(this);

        SettingsButton.OnClickAsObservable().Subscribe(_ =>
        {

        }).AddTo(this);
    }

    /// <summary>
    /// ダイアログ表示
    /// </summary>
    public override void Show(Action closeAction = null)
    {
        base.Show(closeAction);
    }

    /// <summary>
    /// ダイアログを閉じる
    /// </summary>
    public override void Close()
    {
        base.Close();
    }
}
