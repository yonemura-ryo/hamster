using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// ハムスターの管理クラス
/// </summary>
public class HamsterManager : MonoBehaviour
{
    /// <summary>
    /// 出現処理実行間隔秒
    /// </summary>
    [SerializeField] private int appearInterval = 3;

    /// <summary>
    /// バグハムスター出現確率
    /// </summary>
    [SerializeField] private int bugHamsterAppearLottery = 60;
    /// <summary>
    /// 通常ハムスター出現確率
    /// </summary>
    //[SerializeField] private int normalHamsterAppearLottery = 20;
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
    //private int bugHumsterNum = 1;
    private Action<int> addCoin = null;
    private Action<int> consumeFood = null;
    private Action<int> addExp = null;
    
    public Dictionary<int, Hamster> hamsterList = new Dictionary<int, Hamster>();
    /// <summary>
    /// ハムスターの購読処理管理
    /// 　・出現前（シルエット表示）：出現までのカウントダウン
    /// 　・バグ修正中：修正までのカウントダウン
    /// </summary>
    public Dictionary<int, IDisposable> hamsterIDisposableList = new Dictionary<int, IDisposable>();
    private List<FoodArea> foodAreaList = new List<FoodArea>();
    
    /// <summary>
    /// マスタデータ
    /// </summary>
    IReadOnlyDictionary<int, HamsterMaster> hamsterMaster;

    /// <summary>
    /// セーブデータ
    /// </summary>
    private HamsterExistData hamsterExistData = null;
    
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
    public void Initialize(List<FoodArea> foodAreaList, Action<int> addCoin, Action<int> consumeFood, Action<int> addExp)
    {
        // TODO 2回目以降の初期化呼び出しでも正常に処理できるように修正
        this.foodAreaList = foodAreaList;
        this.addCoin = addCoin;
        this.consumeFood = consumeFood;
        this.addExp = addExp;
        // ハム存在データの読み込み
        hamsterExistData = LocalPrefs.Load<HamsterExistData>(SaveData.Key.HamsterExistData);
        if (hamsterExistData == null)
        {
            // 初期データ作成
            hamsterExistData = new HamsterExistData();
            hamsterExistData.hamsterExistDictionary = new SerializableDictionary<int, HamsterData>();
            LocalPrefs.Save(SaveData.Key.HamsterExistData, hamsterExistData);
        }
        // マスタデータ
        this.hamsterMaster = MasterData.DB.HamsterMaster;
        
        // セーブデータから配置
        foreach ((int areaIndex, HamsterData hamsterData) in hamsterExistData.hamsterExistDictionary)
        {
            HamsterMaster hamsterMasterData = MasterData.DB.HamsterMaster[hamsterData.hamsterId];
            HamsterInstantiate(hamsterMasterData, areaIndex, false);
        }
        // ハムスター出現処理
        HamsterAppear();
        // 指定秒ごとに出現処理実行
        Observable.Interval(TimeSpan.FromSeconds(appearInterval))
            .Subscribe(x =>
            {
                // Debug.Log("Interval Time : " + Time.time + ", No : " + x.ToString() +
                //           ", ThreadID : " + System.Threading.Thread.CurrentThread.ManagedThreadId);
                HamsterAppear();
            })
            .AddTo(this);
    }

    /// <summary>
    /// ハムスター出現処理
    /// </summary>
    private void HamsterAppear()
    {
        for (int areaIndex = 0; areaIndex < foodAreaList.Count; areaIndex++)
        {
            // ハムスター配置済み
            if (hamsterList.TryGetValue(areaIndex, out Hamster existHamster))
            {
                continue;
            }
            FoodArea foodArea = foodAreaList[areaIndex];
            int foodId = foodArea.GetExistFoodId();
            // 餌配置済み
            if (foodId > 0)
            {
                // TODO 抽選確率の調整
                Random random = new Random();
                int lottery = random.Next(0,100);
                if(lottery >= bugHamsterAppearLottery){continue;}
                // バグハム出現
                HamsterMaster bugHamsterMaster = LotteryBugHamster();
                HamsterInstantiate(bugHamsterMaster, areaIndex);
                // 餌消費
                consumeFood(areaIndex);
                continue;
            }

            // バグハムスターが存在する
            //if (IsExistBugHamster())
            //{
            //    // TODO 抽選確率の調整
            //    Random random = new Random();
            //    int lottery = random.Next(0,100);
            //    if(lottery >= normalHamsterAppearLottery){continue;}

            //    // 通常ハムスター出現
            //    HamsterMaster normalHamsterMaster = LotteryNormalHamster();
            //    HamsterInstantiate(normalHamsterMaster, areaIndex);
            //    continue;
            //}
        }
    }

    /// <summary>
    /// ハムスターの生成
    /// </summary>
    /// <param name="hamsterMasterData"></param>
    /// <param name="areaIndex"></param>
    /// <param name="bNeedSave"></param>
    private void HamsterInstantiate(HamsterMaster hamsterMasterData, int areaIndex, bool bNeedSave=true)
    {
        GameObject hamsterObject = Instantiate(hamsterPrefab);
        hamsterObject.transform.SetParent(hamstersCanvas.transform);
        Hamster hamster = hamsterObject.GetComponent<Hamster>();
        // TODO 修正時間をセーブデータからの参照に切り替え
        // バグ出現時間
        float bugAppearTime = hamsterMasterData.BugAppearTime;
        hamster.Initialize(
            areaIndex,
            itemPositions[areaIndex],
            ShowDialogByFixedHamster,
            hamsterMasterData.ImagePath,
            hamsterMasterData.NormalImagePath,
            hamsterMasterData.BugId,
            bugAppearTime,
            // TODO 修正時間をセーブデータからの参照に切り替え
            hamsterMasterData.BugFixTime,
            hamsterMasterData.BugFixReward
            );
        // ハムスターの状態を設定
        hamster.MakeSilhouette();
        TimeSpan timeSpan = TimeSpan.FromSeconds(bugAppearTime);
        IDisposable hamsterAppear = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
            .TakeUntil(Observable.Timer(timeSpan))
            .Subscribe(delay =>
            {
                hamster.UpdateFixCountDownText(bugAppearTime - delay - 1);
            },
            () =>
            {
                hamster.MakeBug();
            })
            .AddTo(hamster);

        // 出現中ハムスター管理
        hamsterList[areaIndex] = hamster;
        hamsterIDisposableList[areaIndex] = hamsterAppear;
        
        if (bNeedSave) {
            // セーブ
            HamsterData hamsterData = new HamsterData();
            hamsterData.hamsterId = hamsterMasterData.HamsterId;
            hamsterData.bugFixTime = DateTime.Now.Add(TimeSpan.FromSeconds(hamsterMasterData.BugFixTime));
            hamsterExistData.hamsterExistDictionary.Add(areaIndex, hamsterData);
            LocalPrefs.Save(SaveData.Key.HamsterExistData, hamsterExistData);
        }
    }

    /// <summary>
    /// ハムスター修理時間短縮ダイアログ
    /// </summary>
    /// <param name="hamsterIndex"></param>
    public void ShowDialogByShortenFix(int hamsterIndex)
    {
        //

    }
    
    /// <summary>
    /// ハムスター修理完了ダイアログ
    /// </summary>
    public void ShowDialogByFixedHamster(int hamsterIndex, string imagePath)
    {
        HamsterFixedDialog hamsterFixedDialog = dialogContainer.Show<HamsterFixedDialog>();
        int rewardCoin = hamsterList[hamsterIndex].GetBugFixReward();
        GameObject hamster = hamsterList[hamsterIndex].gameObject;
        hamsterList.Remove(hamsterIndex);
        hamsterFixedDialog.SetHamsterImage(Resources.Load<Sprite>(imagePath));
        Destroy(hamster);
        // ハム修正報酬コイン獲得
        addCoin?.Invoke(rewardCoin);
        addExp?.Invoke(1);
        // セーブ
        hamsterExistData.hamsterExistDictionary.Remove(hamsterIndex);
        LocalPrefs.Save(SaveData.Key.HamsterExistData, hamsterExistData);
    }

    /// <summary>
    /// ハムスターの消え去り
    /// </summary>
    /// <param name="hamsterIndex"></param>
    //public void DisappearHamster(int hamsterIndex)
    //{
    //    GameObject hamster = hamsterList[hamsterIndex].gameObject;
    //    hamsterList.Remove(hamsterIndex);
    //    Destroy(hamster);
    //    // セーブ
    //    hamsterExistData.hamsterExistDictionary.Remove(hamsterIndex);
    //    LocalPrefs.Save(SaveData.Key.HamsterExistData, hamsterExistData);
    //}

    /// <summary>
    /// 通常ハムスターの抽選
    /// </summary>
    /// <returns></returns>
    //private HamsterMaster LotteryNormalHamster()
    //{
    //    // TODO 通常ハムスターの抽選ロジック
    //    int normalHamsterId = 1;
    //    return hamsterMaster[normalHamsterId];
    //}

    /// <summary>
    /// バグハムスターの抽選
    /// </summary>
    /// <returns></returns>
    private HamsterMaster LotteryBugHamster()
    {
        // TODO バグハムスターの抽選ロジック
        Random random = new Random();
        int lottery = random.Next(1,3);
        int bugHamsterId = lottery;
        return hamsterMaster[bugHamsterId];
    }

    /// <summary>
    /// バグハムスターが1匹でも存在するか
    /// </summary>
    /// <returns></returns>
    //private bool IsExistBugHamster()
    //{
    //    foreach (var hamsterDictionary in hamsterList)
    //    {
    //        Hamster hamster = hamsterDictionary.Value;
    //        if (hamster.GetHamsterBugId() > 0)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}
}
