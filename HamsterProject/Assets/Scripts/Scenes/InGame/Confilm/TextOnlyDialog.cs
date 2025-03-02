using System;
using TMPro;
using UniRx;
using UnityEngine;

/// <summary>
/// テキストのみの表示ダイアログ
/// </summary>
public class TextOnlyDialog : DialogBase
{
    [SerializeField] TextMeshProUGUI bodyText = null;
    [SerializeField] CustomButton bodyButton = null;

    // Start is called before the first frame update
    void Start()
    {
        bodyButton.OnClickAsObservable().Subscribe(_ =>
        {
            Close();
        }).AddTo(this);
    }
    public override void Show(Action closeAction = null)
    {
        base.Show(closeAction);
    }

    public override void Close()
    {
        base.Close();
    }

    public void SetTexts(string body, string button = "")
    {
        bodyText.text = body;

        if(button == string.Empty)
        {
            return;
        }

        bodyButton.TMPText.text = button;
    }
}
