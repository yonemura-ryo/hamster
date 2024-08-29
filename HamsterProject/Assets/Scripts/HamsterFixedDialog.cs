using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

// システムシーン常駐の設定ダイアログ
public class HamsterFixedDialog : DialogBase
{
    /// <summary>
    /// ダイアログ表示
    /// </summary>
    public override void Show(Action closeAction)
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
    