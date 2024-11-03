using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DebugDialog : DialogBase
{
    private Action<int> addCoin = null;
    private Action clearData = null;
    private Action<int,int> acquireFood = null;
    private Action<int> addExp = null;
    
    [SerializeField] private Button addCoinButton;
    [SerializeField] private Button removeCoinButton;
    [SerializeField] private Button clearDataButton;
    [SerializeField] private Button acquireFoodButton;
    [SerializeField] private Button notificationButton;
    [SerializeField] private Button addExpButton;

    
    public void Initialize(Action<int> addCoin, Action clearData, Action<int,int> acquireFood, Action<int> addExp)
    {
        this.addCoin = addCoin;
        this.clearData = clearData;
        this.acquireFood = acquireFood;
        this.addExp = addExp;

        // ボタンイベント
        addCoinButton.onClick.AddListener(OnClickAddCoin);
        removeCoinButton.onClick.AddListener(OnClickAddCoin);
        clearDataButton.onClick.AddListener(OnClickClearData);
        acquireFoodButton.onClick.AddListener(OnClickAcquireFood);
        notificationButton.onClick.AddListener(OnClickNotification);
        addExpButton.onClick.AddListener(OnClickAddExp);
    }

    public void OnClickAddCoin()
    {
        // コイン付与処理
        addCoin(1000);
    }

    public void OnClickRemoveCoin()
    {
        // コイン没収処理
        addCoin(-1000);
    }

    public void OnClickClearData()
    {
        // データ削除(PlayerPrefs削除)
        clearData();
    }

    public void OnClickAcquireFood()
    {
        acquireFood(1,5);
        acquireFood(2,5);
        acquireFood(3,5);
        acquireFood(4,5);
        acquireFood(5,5);
    }

    public void OnClickNotification()
    {
        // 10秒後に通知
        LocalPushNotification.RegisterChannel("debugChannel", "PushTest", "デバッグ用の通知");
        LocalPushNotification.AllClear();
        LocalPushNotification.AddSchedule(" テストプッシュ通知", "[デバッグ用]通知時間になりました！", 1, 10, "debugChannel");
    }

    public void OnClickAddExp()
    {
        // 経験値100追加する
        addExp(100);
    }
}