using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Linq;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;
using System.Collections.Generic;

/// <summary>
/// ダイアログ管理クラス
/// </summary>
public class DialogContainer : MonoBehaviour, IDialogContainer
{
    [SerializeField] private Image backGround = null;
    [SerializeField] private DialogBase[] dialogs = null;

    /// <summary> 表示中のダイアログ </summary>
    private List<DialogBase> openDialogs;

    private const float AnimationTIme = 0.3f;

    private void Start()
    {
        backGround.gameObject.SetActive(false);
        ObservablePointerClickTrigger trigger = backGround.gameObject.AddComponent<ObservablePointerClickTrigger>();
        trigger.OnPointerClickAsObservable().Subscribe(e => { OnClickBackGround(); }).AddTo(this);
        openDialogs = new List<DialogBase>();
    }

    /// <summary>
    /// ダイアログ表示
    /// </summary>
    /// <typeparam name="T">Dialog Base</typeparam>
    /// <returns></returns>
    public T Show<T>(Action closeAction= null) where T : DialogBase
    {
        T tmp = (T)dialogs.Where(d => d is T).FirstOrDefault();
        T dialog;
        
        if(tmp != null)
        {
            dialog = Instantiate(tmp, transform).GetComponent<T>();
            dialog.Show(() =>
            {
                closeAction?.Invoke();
                backGround.DOFade(0, AnimationTIme)
                    .OnComplete(() =>
                    {
                        backGround.gameObject.SetActive(false);
                    });
                openDialogs.Remove(dialog);
            });

            backGround.gameObject.SetActive(true);
            backGround.DOFade(0.3f, AnimationTIme);

            openDialogs.Add(dialog);

            return dialog;
        }
        return tmp ;
    }

    /// <summary>
    /// 背景タッチでダイアログを閉じる
    /// </summary>
    private void OnClickBackGround()
    {
        if(openDialogs.Count <= 0)
        {
            return;
        }

        openDialogs[openDialogs.Count - 1].Close();
    }
}
