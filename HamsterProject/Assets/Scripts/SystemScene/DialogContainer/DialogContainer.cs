using DG.Tweening;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ダイアログ管理クラス
/// </summary>
public class DialogContainer : MonoBehaviour, IDialogContainer
{
    [SerializeField] private Image backGround = null;
    [SerializeField] private DialogBase[] dialogs = null;

    private const float AnimationTIme = 0.3f;

    private void Start()
    {
        backGround.gameObject.SetActive(false);
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
            });

            backGround.gameObject.SetActive(true);
            backGround.DOFade(0.3f, AnimationTIme);

            return dialog;
        }
        return tmp ;
    }
}
