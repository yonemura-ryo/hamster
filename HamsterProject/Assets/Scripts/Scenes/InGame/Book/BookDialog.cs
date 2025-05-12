using System;
using TMPro;
using UniRx;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 図鑑
/// </summary>
public class BookDialog : DialogBase
{
    [SerializeField] RectTransform hamsterBookContent;
    [SerializeField] GameObject hamsterBookContentPrefab;
    // TODO 仮として1つのAspriteからSpriteを取得する
    [SerializeField] GameObject asepriteObject;

    private IDialogContainer dialogContainer;
    IReadOnlyDictionary<int, HamsterMaster> hamsterMasters;

    // Start is called before the first frame update
    void Start()
    {
        //closeButton.OnClickAsObservable().Subscribe(_ =>
        //{
        //    Close();
        //}).AddTo(this);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize(
        IDialogContainer dialogContainer,
        HamsterCapturedListData hamsterCapturedListData
        )
    {
        // TODO 初期化
        this.dialogContainer = dialogContainer;

        // マスタ読み込み
        hamsterMasters = MasterData.DB.HamsterMaster;
        SpriteRenderer sr = asepriteObject.GetComponent<SpriteRenderer>();
        // 現在表示されているSpriteを取得
        Sprite firstFrame = sr.sprite;

        // BugId が 99 以外で HamsterId 昇順の配列を取得
        var filteredSortedBugHamsters = hamsterMasters
            .Values
            .Where(h => h.BugId != 99)
            .OrderBy(h => h.HamsterId)
            .ToArray();
        // 図鑑のContentを作成する
        foreach (var hamsterMasterObj in filteredSortedBugHamsters.Select((hamster, index) => new { hamster, index }))
        {
            bool isCaptured = hamsterCapturedListData.capturedDataDictionary.ContainsKey(hamsterMasterObj.hamster.HamsterId + ":1");
            int capturedCount = 0;
            if (isCaptured)
            {
                capturedCount = hamsterCapturedListData.capturedDataDictionary[hamsterMasterObj.hamster.HamsterId + ":1"].capturedCount;
            }
            HamsterDetail hamsterDetail = new HamsterDetail(
                hamsterMasterObj.index + 1,
                isCaptured,
                hamsterMasterObj.hamster.Name,
                hamsterMasterObj.hamster.Rare,
                hamsterMasterObj.hamster.ColorTypeId,
                "サンプルテキスト.cm",
                "サンプルテキスト",
                "サンプルテキスト",
                capturedCount,
                "サンプルテキスト",
                "サンプルテキスト",
                firstFrame
                );
            GameObject hamsterBookContentObject = Instantiate(hamsterBookContentPrefab, Vector3.zero, Quaternion.identity, hamsterBookContent);
            hamsterBookContentObject.GetComponent<HamsterBookContent>().Initialize(
                //hamsterMasterObj.index + 1,
                //hamsterMasterObj.hamster.Name,
                //firstFrame,
                //isCaptured,
                hamsterDetail,
                OpenDetailDialog
            );
        }

        // デバッグ用に捕獲済みハムスター出力
        //foreach (var ham in hamsterCapturedListData.capturedDataDictionary)
        //{
        //    Debug.Log(ham.Value.hamsterId + ":" + ham.Value.colorId);
        //}
    }

    public override void Show(Action closeAction = null)
    {
        base.Show(closeAction);
    }

    public override void Close()
    {
        base.Close();
    }

    private void OnCloseNextDialog()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 詳細ダイアログ開く
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="sprite"></param>
    private void OpenDetailDialog(HamsterDetail hamsterDetail)
    {
        BookDetailDialog bookDetailDialog = dialogContainer?.Show<BookDetailDialog>(OnCloseNextDialog);
        bookDetailDialog?.Initialize(hamsterDetail);
        gameObject.SetActive(false);
    }
}
