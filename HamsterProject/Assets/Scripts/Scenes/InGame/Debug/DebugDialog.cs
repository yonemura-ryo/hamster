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
    public void Initialize(Action<int> addCoin, Action clearData)
    {
        this.addCoin = addCoin;
        this.clearData = clearData;
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
}