using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class InGameController : SceneControllerBase
{
    [SerializeField] private InGamePresenter inGamePresenter = null;
    [SerializeField] private DialogContainer dialogContainer = null;
    [SerializeField] private HamsterManager hamsterManager = null;
    
    /// <summary>
    /// 表示オブジェクト
    /// </summary>
    [SerializeField] private List<Facility> facilities = new List<Facility>();
    [SerializeField] private List<FoodArea> foodAreas = new List<FoodArea>();

    /// <summary>
    /// マスターデータ
    /// </summary>
    private IReadOnlyDictionary<int, FacilityMaster> FacilityMaster = null;
    private IReadOnlyDictionary<int, FoodMaster> FoodMaster = null;
    private IReadOnlyDictionary<int, UserRankMaster> UserRankMaster = null;
    
    /// <summary>
    /// [セーブデータ]施設データ
    /// </summary>
    private FacilityListData facilityListData = null;
    public FacilityListData FacilityListDataReadOnly => facilityListData;
    /// <summary>
    /// [セーブデータ]ユーザー基本データ
    /// </summary>
    private static UserCommonData userCommonData = null;
    /// <summary>
    /// [セーブデータ]参照用のユーザー基本データ
    /// TODO ReactivePropertyでうまく作ってもいいかもしれない
    /// </summary>
    public static UserCommonData userCommonDataReadOnly => userCommonData;
    /// <summary>
    /// [セーブデータ]餌所持データ
    /// </summary>
    private HavingFoodData havingFoodData = null;
    /// <summary>
    /// [セーブデータ]捕獲済みハムデータ
    /// </summary>
    private HamsterCapturedListData hamsterCapturedListData = null;
    
    /// <summary>  ダイアログコンテナ公開用 </summary>
    public IDialogContainer DialogContainer => dialogContainer;

    /// <summary>
    /// ������
    /// </summary>
    protected override void Initialize()
    {
        // マスターデータの読み込み
        FacilityMaster = MasterData.DB.FacilityMaster;
        FoodMaster = MasterData.DB.FoodMaster;
        UserRankMaster = MasterData.DB.UserRankMaster;

        // ユーザーデータの読み込み
        userCommonData =  LocalPrefs.Load<UserCommonData>(SaveData.Key.UserCommonData);
        // 初期ユーザーデータの作成
        if (userCommonData == null)
        {
            userCommonData = new UserCommonData();
            userCommonData.coinCount = 0;
            userCommonData.exp = 0;
            userCommonData.userRank = 1;
            LocalPrefs.Save(SaveData.Key.UserCommonData, userCommonData);
        }
        // 施設データの読み込み
        facilityListData =  LocalPrefs.Load<FacilityListData>(SaveData.Key.FacilityList);
        if (facilityListData == null)
        {
            facilityListData = InitializeFacilityListData(FacilityMaster.Count);
        }
        // 所持餌データの読み込み
        havingFoodData = LocalPrefs.Load<HavingFoodData>(SaveData.Key.HavingFoodData);
        if (havingFoodData == null)
        {
            havingFoodData = new HavingFoodData();
            havingFoodData.havingFoodDictionary = new SerializableDictionary<int, FoodData>();
            LocalPrefs.Save(SaveData.Key.HavingFoodData, havingFoodData);
        }

        // 捕獲済みハムデータの読み込み
        hamsterCapturedListData = LocalPrefs.Load<HamsterCapturedListData>(SaveData.Key.HamsterCapturedListData);
        if(hamsterCapturedListData == null)
        {
            hamsterCapturedListData = new HamsterCapturedListData();
            hamsterCapturedListData.capturedDataDictionary = new SerializableDictionary<string, HamsterCapturedData>();
            LocalPrefs.Save(SaveData.Key.HamsterCapturedListData, hamsterCapturedListData);
        }

        InGameModel model = new InGameModel(SystemScene.Instance.SceneTransitioner);
        SystemScene systemScene = SystemScene.Instance;
        inGamePresenter.Initialize(
            model,
            dialogContainer,
            systemScene.SoundPlayer,
            systemScene.SceneTransitioner,
            userCommonDataReadOnly,
            AddCoin,
            AcquireFood,
            FacilityListDataReadOnly,
            UpdateFacilityLevel
            );
        // 施設の初期化
        foreach ((int key,FacilityData facilityData) in facilityListData.facilityDictionary)
        {
            // facilityIdが不正値の場合はスキップ
            if(facilityData.facilityId <= 0){continue;}
            FacilityMaster facilityMaster = FacilityMaster[facilityData.facilityId];
            // TODO リスト存在チェック
            facilities[facilityData.facilityId -1].Initialize(
                facilityData.facilityId,
                facilityMaster.Name,
                facilityData.level,
                ShowDialogByLevelUpFacility
                );
        }
        // 餌場の初期化
        for (int i = 0; i < foodAreas.Count; i++)
        {
            // TODO 解放されてる餌場のみに限定する
            foodAreas[i].Initialize(i, ShowDialogByFoodArea);
        }
        // ハムの初期化
        hamsterManager.Initialize(foodAreas, AddCoin, ConsumeFood, AddExp, SaveCapturedHamster);
    }

    /// <summary>
    /// 施設データの初期化
    /// </summary>
    /// <returns></returns>
    public FacilityListData InitializeFacilityListData(int facilityCount)
    {
        FacilityListData facilityListData = new FacilityListData();
        // TODO マスタから施設数をとる
        facilityListData.facilityDictionary = new SerializableDictionary<int, FacilityData>();
        for (int i = 0; i < facilityCount; i++)
        {
            FacilityData facilityData = new FacilityData();
            facilityData.facilityId = i + 1;
            facilityData.level = 0;
            facilityListData.facilityDictionary[facilityData.facilityId] = facilityData;
        }
        
        LocalPrefs.Save(SaveData.Key.FacilityList, facilityListData);
        return facilityListData;
    }

    /// <summary>
    /// コインの増減
    /// </summary>
    /// <param name="addCoinCount"></param>
    public void AddCoin(int addCoinCount)
    {
        userCommonData.coinCount += addCoinCount;
        // 0以下は0にする(ここでは不足チェックしない)
        if (userCommonData.coinCount < 0)
        {
            userCommonData.coinCount = 0;
        }
        inGamePresenter.UpdateCoinText(userCommonData.coinCount);
        // ユーザーデータセーブ
        LocalPrefs.Save(SaveData.Key.UserCommonData, userCommonData);
    }

    /// <summary>
    /// 経験値付与
    /// </summary>
    /// <param name="addExp"></param>
    public void AddExp(int addExp)
    {
        userCommonData.exp += addExp;
        // ユーザーランク上昇判定
        bool isRankUp = true;
        List<int> functonReleaseIds = new List<int>();
        while (isRankUp)
        {
            if (UserRankMaster.ContainsKey(userCommonData.userRank + 1))
            {
                UserRankMaster nextRankMaster = UserRankMaster[userCommonData.userRank + 1];
                if(nextRankMaster.Exp <= userCommonData.exp)
                {
                    // レベル上昇
                    userCommonData.userRank = nextRankMaster.Rank;
                    if(nextRankMaster.FunctionReleaseId > 0)
                    {
                        functonReleaseIds.Add(nextRankMaster.FunctionReleaseId);
                    }
                }
                else
                {
                    // 経験値未達
                    isRankUp = false;
                }
            }
            else
            {
                // 次のランクがない = 最大レベルとみなし経験値上限張りつきにする
                UserRankMaster currentRankMaster = UserRankMaster[userCommonData.userRank];
                userCommonData.exp = currentRankMaster.Exp;
                isRankUp = false;
            }
        }
        // TODO ユーザーランク上昇通知ダイアログ
        inGamePresenter.UpdateRankText(userCommonData.userRank);
        // TODO 機能解放実行処理
        foreach (var functionReleaseId in functonReleaseIds)
        {
            //
        }
        // ユーザーデータセーブ
        LocalPrefs.Save(SaveData.Key.UserCommonData, userCommonData);
    }

    /// <summary>
    /// 施設レベルアップ
    /// </summary>
    /// <param name="facilityId"></param>
    /// <param name="spendCoin"></param>
    /// <returns></returns>
    public bool UpdateFacilityLevel(int facilityId, int spendCoin)
    {
        // コイン足りない
        if(userCommonData.coinCount < spendCoin) { return false; }
        // コイン消費
        AddCoin(-spendCoin);
        // 施設レベルアップ
        facilityListData.facilityDictionary[facilityId].level++;
        // 施設データセーブ
        LocalPrefs.Save(SaveData.Key.FacilityList, facilityListData);
        // facilityの変数更新
        facilities[facilityId -1].UpdateMemberValue(facilityListData.facilityDictionary[facilityId].level);
        return true;
    }
    
    /// <summary>
    /// 施設レベルアップダイアログ
    /// </summary>
    /// <param name="facilityId"></param>
    public void ShowDialogByLevelUpFacility(int facilityId)
    {
        FacilityLevelUpDialog facilityLevelUpDialog = dialogContainer.Show<FacilityLevelUpDialog>();
        facilityLevelUpDialog.Initialize(
            facilityId,
            FacilityMaster[facilityId].Name,
            FacilityMaster[facilityId].LevelType,
            facilityListData.facilityDictionary[facilityId].level,
            facilities[facilityId -1].FacilitySprite,
            UpdateFacilityLevel
            );
    }

    /// <summary>
    /// 餌使用配置
    /// </summary>
    /// <param name="foodAreaId"></param>
    /// <param name="foodId"></param>
    public void UseFood(int foodAreaId, int foodId)
    {
        // TODO 餌の配置と消費時間管理
        FoodMaster foodMasterData = FoodMaster[foodId];
        foodAreas[foodAreaId].SetFood(foodId, foodMasterData.LotteryId);
        
        // TODO 一旦餌の消滅はなしにする
        //Observable.Timer (TimeSpan.FromMilliseconds (foodMasterData.Duration * 1000))
        //    .Subscribe (delay => {
        //        foodAreas[foodAreaId].SetEmptyFood();
        //    });
        // 餌消費
        havingFoodData.havingFoodDictionary[foodId].count--;
        LocalPrefs.Save(SaveData.Key.HavingFoodData, havingFoodData);
    }

    /// <summary>
    /// 餌配置ダイアログ
    /// </summary>
    /// <param name="foodAreaId"></param>
    public void ShowDialogByFoodArea(int foodAreaId)
    {
        // TODO 餌選択ダイアログ
        FoodAreaDialog foodAreaDialog = dialogContainer.Show<FoodAreaDialog>();
        foodAreaDialog.Initialize(
            foodAreaId,
            havingFoodData.havingFoodDictionary,
            UseFood
            );
    }

    /// <summary>
    /// 餌の獲得
    /// </summary>
    /// <param name="foodId"></param>
    /// <param name="num"></param>
    public void AcquireFood(int foodId, int num)
    {
        if (!havingFoodData.havingFoodDictionary.TryGetValue(foodId, out FoodData currentFoodData))
        {
            var foodData = new FoodData();
            foodData.foodId = foodId;
            // 所持数マイナスを防ぐ(ここではハンドリングしない)
            if(num < 0)
            {
                num = 0;
            }
            foodData.count = num;
            havingFoodData.havingFoodDictionary[foodId] = foodData;
        }
        else
        {
            havingFoodData.havingFoodDictionary[foodId].count += num;
        }
        LocalPrefs.Save(SaveData.Key.HavingFoodData, havingFoodData);
    }

    /// <summary>
    /// ハムスター出現による餌消費
    /// </summary>
    /// <param name="foodAreaId"></param>
    public void ConsumeFood(int foodAreaId)
    {
        foodAreas[foodAreaId].SetEmptyFood();
        // TODO 餌消費Timerの削除
    }

    /// <summary>
    /// 獲得ハムスターの保存
    /// </summary>
    /// <param name="hamsterId"></param>
    /// <param name="colorId"></param>
    public void SaveCapturedHamster(int hamsterId, int colorId)
    {
        string key = hamsterId + ":" + colorId;
        if (hamsterCapturedListData.capturedDataDictionary.ContainsKey(key))
        {
            // カウントアップ
            HamsterCapturedData hamsterCapturedData = hamsterCapturedListData.capturedDataDictionary[key];
            hamsterCapturedData.capturedCount++;
            hamsterCapturedListData.capturedDataDictionary[key] = hamsterCapturedData;
        } else
        {
            // 初回入手
            HamsterCapturedData hamsterCapturedData = new HamsterCapturedData();
            hamsterCapturedData.hamsterId = hamsterId;
            hamsterCapturedData.colorId = colorId;
            hamsterCapturedData.capturedCount = 1;
            hamsterCapturedListData.capturedDataDictionary[key] = hamsterCapturedData;
        }
        LocalPrefs.Save(SaveData.Key.HamsterCapturedListData, hamsterCapturedListData);
    }

    /// <summary>
    /// デバッグメニューの表示
    /// </summary>
    public void OnClickDebugDialog()
    {
        DebugDialog debugDialog = dialogContainer.Show<DebugDialog>();
        debugDialog.Initialize(
            AddCoin,
            DebugClearPlayerPrefs,
            AcquireFood,
            AddExp
            );
    }

    /// <summary>
    /// [デバッグ]PlayerPrefsの削除
    /// </summary>
    public void DebugClearPlayerPrefs()
    {
        // PlayerPrefs削除
        LocalPrefs.DeleteAll();
        // 初期化を走らせておく
        Initialize();
    }
}
