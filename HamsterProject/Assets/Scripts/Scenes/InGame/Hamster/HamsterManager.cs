using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ハムスターの管理クラス
/// </summary>
public class HamsterManager : MonoBehaviour
{
    /// <summary>
    /// ハムスターを描画するキャンバス
    /// </summary>
    [SerializeField] private GameObject hamstersCanvas;
    
    /// <summary>
    /// ハムスターのprefab
    /// </summary>
    [SerializeField]private GameObject hamsterPrefab;

    [SerializeField] private List<Transform> itemPositions;
    
    [SerializeField] private DialogContainer dialogContainer = null;
    
    private int itemPositionNum = 3;
    private int bugHumsterNum = 1;
    private Action<int> addCoin = null;
    
    // TODO Listにすると削除時indexずれるので配列にする
    public List<Hamster> hamsterList = new List<Hamster>();
    
    /// <summary>  ダイアログコンテナ公開用 </summary>
    public IDialogContainer DialogContainer => dialogContainer;

    private void Start()
    {
        // TODO アイテム位置の処理は規定値からとるように修正する
        if (itemPositions.Count < itemPositionNum)
        {
            var makeCount = itemPositionNum - itemPositions.Count;
            for (int i = 0; i < makeCount; i++)
            {
                itemPositions.Add(this.transform);
            }
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize(Action<int> addCoin)
    {
        // TODO 2回目以降の初期化呼び出しでも正常に処理できるように修正
        this.addCoin = addCoin;
        IReadOnlyDictionary<int, HamsterMaster> HamsterMaster = MasterData.DB.HamsterMaster;
        // TODO 通常ハムスター出現処理つくる
        var normalHamID = 1;
        var hamsterMaster = HamsterMaster[normalHamID];
        // TODO 一旦生成してみる
        for (int i = 0; i < itemPositionNum - bugHumsterNum; i++)
        {
            // 通常ハム
            GameObject hamsterObject = Instantiate(hamsterPrefab);
            hamsterObject.transform.SetParent(hamstersCanvas.transform);
            Hamster hamster = hamsterObject.GetComponent<Hamster>();
                hamster.Initialize(
                    hamsterList.Count,
                itemPositions[i],
                ShowDialogByFixedHamster,
                hamsterMaster.ImagePath,
                hamsterMaster.BugFixTime,
                hamsterMaster.BugId);
            hamsterList.Add(hamster);
        }

        // TODO バグハムスター出現処理つくる
        var bugHumID = 2;
        hamsterMaster = HamsterMaster[bugHumID];

        for (int i = 0; i < bugHumsterNum; i++)
        {
            // バグハム
            GameObject bugHamsterObject = Instantiate(hamsterPrefab);
            bugHamsterObject.transform.SetParent(hamstersCanvas.transform);
            Hamster bugHamster = bugHamsterObject.GetComponent<Hamster>();
                bugHamster.Initialize(
                    hamsterList.Count,
                itemPositions[itemPositionNum - bugHumsterNum + i],
                ShowDialogByFixedHamster,
                hamsterMaster.ImagePath,
                hamsterMaster.BugFixTime,
                hamsterMaster.BugId);
            hamsterList.Add(bugHamster);
        }
    }

    /// <summary>
    /// ハムスター修理完了ダイアログ
    /// </summary>
    public void ShowDialogByFixedHamster(int hamsterIndex, string imagePath)
    {
        HamsterFixedDialog hamsterFixedDialog = dialogContainer.Show<HamsterFixedDialog>();
        GameObject hamster = hamsterList[hamsterIndex].gameObject;
        hamsterList.RemoveAt(hamsterIndex);
        hamsterFixedDialog.SetHamsterImage(Resources.Load<Sprite>(imagePath));
        Destroy(hamster);
        // TODO マスタからハムの金額参照
        addCoin?.Invoke(100);
    }
}
