using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Random = System.Random;
using System.Linq;

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
    private Action<int, int> saveCapturedHamster = null;
    
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
    IReadOnlyDictionary<string, HamsterColorTypeMaster> hamsterColorTypeMaster;

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
    public void Initialize(List<FoodArea> foodAreaList, Action<int> addCoin, Action<int> consumeFood, Action<int> addExp, Action<int, int> saveCapturedHamster)
    {
        // TODO 2回目以降の初期化呼び出しでも正常に処理できるように修正
        this.foodAreaList = foodAreaList;
        this.addCoin = addCoin;
        this.consumeFood = consumeFood;
        this.addExp = addExp;
        this.saveCapturedHamster = saveCapturedHamster;
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
        this.hamsterColorTypeMaster = MasterData.DB.HamsterColorTypeMaster;
        
        // セーブデータから配置
        foreach ((int areaIndex, HamsterData hamsterData) in hamsterExistData.hamsterExistDictionary)
        {
            HamsterMaster hamsterMasterData = MasterData.DB.HamsterMaster[hamsterData.hamsterId];
            // TODO セーブデータからcolorID取得
            HamsterInstantiate(hamsterMasterData, areaIndex, 1, hamsterData, false);
        }
        // ハムスター出現処理
        HamsterAppear();
        // 指定秒ごとに出現処理実行
        Observable.Interval(TimeSpan.FromSeconds(appearInterval))
            .Subscribe(x =>
            {
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
                // 色抽選
                var targetColorTypeMasters = hamsterColorTypeMaster
                    .Where(x => x.Value.ColorTypeId == bugHamsterMaster.ColorTypeId)
                ;
                // TODO 色抽選確率の調整
                int colorTypeCount = targetColorTypeMasters.Count();
                int colorLottery = random.Next(0, colorTypeCount-1);
                HamsterColorTypeMaster targetColorTypeMaster = targetColorTypeMasters
                    .OrderBy(x => x.Value.ColorId)
                    .Skip(colorLottery)
                    .FirstOrDefault()
                    .Value
                    ;
                HamsterInstantiate(bugHamsterMaster, areaIndex, targetColorTypeMaster.ColorId);
                // 餌消費
                consumeFood(areaIndex);
                continue;
            }
        }
    }

    /// <summary>
    /// ハムスターの生成
    /// </summary>
    /// <param name="hamsterMasterData"></param>
    /// <param name="areaIndex">出現するインデックス</param>
    /// <param name="hamsterSaveData">生成に使用するセーブデータ</param>
    /// <param name="bNeedSave">TODO セーブするかフラグ。いらない気がする</param>
    private void HamsterInstantiate(HamsterMaster hamsterMasterData, int areaIndex, int colorId, HamsterData hamsterSaveData=null, bool bNeedSave=true)
    {
        GameObject hamsterObject = Instantiate(hamsterPrefab);
        hamsterObject.transform.SetParent(hamstersCanvas.transform);
        Hamster hamster = hamsterObject.GetComponent<Hamster>();
        // バグ出現時間
        float bugAppearTime = hamsterMasterData.BugAppearTime;
        float bugFixTime = hamsterMasterData.BugFixTime;
        bool isStartFix = false;
        if(hamsterSaveData != null)
        {
            TimeSpan bugAppearTimeSpan = DateTime.Parse(hamsterSaveData.bugAppearTime) - DateTime.Now;
            bugAppearTime = bugAppearTimeSpan.Seconds;
            // 修正開始後か？
            if (hamsterSaveData.bugFixTime != DateTime.MaxValue.ToString())
            {
                isStartFix = true;
                TimeSpan bugFixTimeSpan = DateTime.Parse(hamsterSaveData.bugFixTime) - DateTime.Now;
                bugFixTime = bugFixTimeSpan.Seconds;
            }
        }
        hamster.Initialize(
            hamsterMasterData.HamsterId,
            areaIndex,
            itemPositions[areaIndex],
            ShowDialogByFixedHamster,
            hamsterMasterData.ImagePath,
            hamsterMasterData.NormalImagePath,
            hamsterMasterData.BugId,
            bugFixTime,
            hamsterMasterData.BugFixReward,
            colorId,
            (index, bugFixTime) => StartHamsterBugFix(index, bugFixTime)
            );
        // 出現中ハムスター管理
        hamsterList[areaIndex] = hamster;
        // ハムスターの状態を設定
        if (bugAppearTime > 0)
        {
            // 出現前処理
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
            hamsterIDisposableList[areaIndex] = hamsterAppear;
        }
        else if (!isStartFix)
        {
            // 出現済みバグ修正開始前
            hamster.MakeBug();
        }
        else
        {
            // バグ修正開始済み
            hamster.MakeBugFixing();
            StartHamsterBugFix(areaIndex, bugFixTime, false);
        }
        
                
        if (bNeedSave && (hamsterSaveData == null)) {
            // セーブ
            HamsterData hamsterData = new HamsterData();
            hamsterData.hamsterId = hamsterMasterData.HamsterId;
            hamsterData.colorId = colorId;
            hamsterData.bugFixTime = DateTime.MaxValue.ToString();
            hamsterData.bugAppearTime = DateTime.Now.Add(TimeSpan.FromSeconds(bugAppearTime)).ToString();
            hamsterExistData.hamsterExistDictionary.Add(areaIndex, hamsterData);
            LocalPrefs.Save(SaveData.Key.HamsterExistData, hamsterExistData);
        }
    }

    /// <summary>
    /// ハムスターバグ修正開始
    /// </summary>
    /// <param name="hamsterIndex"></param>
    /// <param name="bugFixTime"></param>
    public void StartHamsterBugFix(int hamsterIndex, float bugFixTime, bool bNeedSave=true)
    {
        Hamster hamster = hamsterList[hamsterIndex];

        TimeSpan timeSpan = TimeSpan.FromSeconds(bugFixTime);
        IDisposable hamsterBugFix = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
            .TakeUntil(Observable.Timer(timeSpan))
            .Subscribe(delay =>
            {
                hamster.UpdateFixCountDownText(bugFixTime - delay - 1);
            },
            () =>
            {
                hamster.MakeBugFixed();
            })
            .AddTo(hamster);
        // TODO 古いIDisposableを消す
        hamsterIDisposableList[hamsterIndex] = hamsterBugFix;

        // セーブ
        if (bNeedSave)
        {
            HamsterData hamsterData = hamsterExistData.hamsterExistDictionary[hamsterIndex];
            hamsterData.bugFixTime = DateTime.Now.Add(TimeSpan.FromSeconds(bugFixTime)).ToString();
            hamsterExistData.hamsterExistDictionary[hamsterIndex] = hamsterData;
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
    public void ShowDialogByFixedHamster(int hamsterIndex, string imagePath, int colorId)
    {
        HamsterFixedDialog hamsterFixedDialog = dialogContainer.Show<HamsterFixedDialog>();
        int hamsterId = hamsterList[hamsterIndex].GetHamsterId();
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
        // 修正済みハムスターデータセーブ
        saveCapturedHamster(hamsterId, colorId);
    }

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
}
