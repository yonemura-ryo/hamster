using System;
using UniRx;
using UnityEngine;

/// <summary>
/// ダイアログベースクラス.
/// </summary>
public class DialogBase : MonoBehaviour
{
    [SerializeField] protected CustomButton closeButton = null;

    // ダイアログを閉じる時のcall back.
    protected Action closeAction;

    /// <summary>
    /// ダイアログ表示
    /// </summary>
    public virtual void Show(Action closeAction = null)
    {
        if(closeButton != null)
        {
            this.closeAction = closeAction;
            closeButton.OnClickAsObservable().Subscribe(_ =>
            {
                Close();
            }).AddTo(this);
        }
    }

    /// <summary>
    /// ダイアログを閉じる
    /// </summary>
    public virtual void Close()
    {
        closeAction?.Invoke();
        Destroy(gameObject);
    }
}