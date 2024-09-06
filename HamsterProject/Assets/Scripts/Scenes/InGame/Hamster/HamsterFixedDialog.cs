using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

// システムシーン常駐の設定ダイアログ
public class HamsterFixedDialog : DialogBase
{
    [SerializeField] Image hamsterImage;
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

    public void SetHamsterImage(Sprite sprite)
    {
        hamsterImage.sprite = sprite;
    }
}
    