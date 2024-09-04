using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1ハムスターのコントローラー
/// </summary>
public class HamsterController : MonoBehaviour
{
    RectTransform rectTransform;
    [SerializeField] private HamsterPresenter hamsterPresenter;
    private Action finishFixAction;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="hamsterMaster"></param>
    /// <param name="position"></param>
    public void Initialize(HamsterMaster hamsterMaster, Transform position, Action<GameObject> finishFixAction, HamsterMaster fixedHamssterMaster=null)
    {
        HamsterModel hamsterModel = new HamsterModel(hamsterMaster, fixedHamssterMaster);
        this.finishFixAction = finishFixAction;
        hamsterPresenter.Initialize(hamsterModel, this.finishFixAction);
        // TODO 仮 位置を0に
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localPosition = position.localPosition;
    }

    public void OnClickHamster()
    {
        hamsterPresenter.OnClickHamster();
    }
}
