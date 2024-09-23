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
    
    [SerializeField] private Button addCoinButton;
    [SerializeField] private Button removeCoinButton;
    [SerializeField] private Button clearDataButton;
    [SerializeField] private Button acquireFoodButton;

    
    public void Initialize(Action<int> addCoin, Action clearData, Action<int,int> acquireFood)
    {
        this.addCoin = addCoin;
        this.clearData = clearData;
        this.acquireFood = acquireFood;
        
        // ボタンイベント
        addCoinButton.onClick.AddListener(OnClickAddCoin);
        removeCoinButton.onClick.AddListener(OnClickAddCoin);
        clearDataButton.onClick.AddListener(OnClickClearData);
        acquireFoodButton.onClick.AddListener(OnClickAcquireFood);

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
}