using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using TMPro;

public class ConfilmDialog : DialogBase
{
    [SerializeField] CustomButton ApproveButton = null;
    [SerializeField] CustomButton CancelButton = null;
    [SerializeField] TextMeshProUGUI ConfilmText = null;

    public void Initialize(string confilmMessage, Action approveAction, Action cancelAction)
    {
        ConfilmText.text = confilmMessage;
        ApproveButton.OnClickAsObservable().Subscribe(_ =>
        {
            approveAction?.Invoke();
            Close();
        }).AddTo(this);
        CancelButton.OnClickAsObservable().Subscribe(_ =>
        {
            cancelAction?.Invoke();
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
}
