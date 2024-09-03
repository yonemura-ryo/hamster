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
    
    private List<GameObject> hamsterList = new List<GameObject>();
    
    /// <summary>  ダイアログコンテナ公開用 </summary>
    public IDialogContainer DialogContainer => dialogContainer;

    private void Start()
    {
        if (itemPositions.Count < itemPositionNum)
        {
            var makeCount = itemPositionNum - itemPositions.Count;
            for (int i = 0; i < makeCount; i++)
            {
                itemPositions.Add(this.transform);
            }
        }
        Initialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        var normalHamID = 1; // TODO 仮
        // TODO 一旦生成してみる
        for (int i = 0; i < itemPositionNum - bugHumsterNum; i++)
        {
            // 通常ハム
            GameObject hamster = Instantiate(hamsterPrefab);
            hamster.transform.SetParent(hamstersCanvas.transform);
            hamster.GetComponent<HamsterController>().Initialize(
                MasterData.DB.HamsterMaster[normalHamID],
                itemPositions[i],
                () => ShowDialogByFixedHamster(hamster)
                );
            
            // TODO 仮
            hamsterList.Add(hamster);
        }

        var bugHumID = 2; // TODO 仮
        for (int i = 0; i < bugHumsterNum; i++)
        {
            // バグハム
            GameObject bugHamster = Instantiate(hamsterPrefab);
            bugHamster.transform.SetParent(hamstersCanvas.transform);
            bugHamster.GetComponent<HamsterController>().Initialize(
                MasterData.DB.HamsterMaster[bugHumID],
                itemPositions[itemPositionNum - bugHumsterNum + i],
                () => ShowDialogByFixedHamster(bugHamster),
                MasterData.DB.HamsterMaster[normalHamID]
                );
            // TODO 仮
            hamsterList.Add(bugHamster);
        }
    }

    /// <summary>
    /// ハムスター修理完了ダイアログ
    /// </summary>
    /// <param name="hamster"></param>
    public void ShowDialogByFixedHamster(GameObject hamster)
    {
        HamsterFixedDialog hamsterFixedDialog = dialogContainer.Show<HamsterFixedDialog>();
        hamsterFixedDialog.SetHamsterImage(hamster.GetComponent<HamsterPresenter>().GetFixedHamsterImage());
        Destroy(hamster);
    }
}
