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
    
    /// <summary>
    /// セーブデータ
    /// </summary>
    // TODO 一時的[SerializeField] 外す
    [SerializeField] private FacilityListData facilityListData = null;
    private UserCommonData userCommonData = null;
    // TODO 一時的[SerializeField] 外す
    [SerializeField] private HavingFoodData havingFoodData = null;
    
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

        // ユーザーデータの読み込み
        userCommonData =  LocalPrefs.Load<UserCommonData>(SaveData.Key.UserCommonData);
        // 初期ユーザーデータの作成
        if (userCommonData == null)
        {
            userCommonData = new UserCommonData();
            userCommonData.coinCount = 0;
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
        // DebugAcquireFood(1, 5);

        InGameModel model = new InGameModel(SystemScene.Instance.SceneTransitioner);
        inGamePresenter.Initialize(model, dialogContainer, userCommonData.coinCount);
        // 施設の初期化
        foreach (var facilityData in facilityListData.facilities)
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
        hamsterManager.Initialize(foodAreas,AddCoin,ConsumeFood);
    }

    /// <summary>
    /// 施設データの初期化
    /// </summary>
    /// <returns></returns>
    public FacilityListData InitializeFacilityListData(int facilityCount)
    {
        FacilityListData facilityListData = new FacilityListData();
        // TODO マスタから施設数をとる
        facilityListData.facilities = new FacilityData[facilityCount + 1];
        for (int i = 0; i < facilityCount; i++)
        {
            FacilityData facilityData = new FacilityData();
            facilityData.facilityId = i + 1;
            facilityData.level = 0;
            facilityListData.facilities[facilityData.facilityId] = facilityData;
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
        facilityListData.facilities[facilityId].level++;
        // 施設データセーブ
        LocalPrefs.Save(SaveData.Key.FacilityList, facilityListData);
        // facilityの変数更新
        facilities[facilityId -1].UpdateMemberValue(facilityListData.facilities[facilityId].level);
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
            facilityListData.facilities[facilityId].level,
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
        foodAreas[foodAreaId].SetFood(foodId);
        FoodMaster foodMasterData = FoodMaster[foodId];
        Observable.Timer (TimeSpan.FromMilliseconds (foodMasterData.Duration * 1000))
            .Subscribe (delay => {
                foodAreas[foodAreaId].SetEmptyFood();
            });
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

    public void ConsumeFood(int foodAreaId)
    {
        foodAreas[foodAreaId].SetEmptyFood();
        // TODO 餌消費Timerの削除
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
            DebugAcquireFood
            );
    }

    /// <summary>
    /// [デバッグ]PlayerPrefsの削除
    /// </summary>
    public void DebugClearPlayerPrefs()
    {
        // PlayerPrefs削除
        PlayerPrefs.DeleteAll();
        // 初期化を走らせておく
        Initialize();
    }

    /// <summary>
    /// [デバッグ]餌付与
    /// </summary>
    /// <param name="foodId"></param>
    /// <param name="num"></param>
    public void DebugAcquireFood(int foodId, int num)
    {
        if (!havingFoodData.havingFoodDictionary.TryGetValue(foodId, out FoodData currentFoodData))
        {
            var foodData = new FoodData();
            foodData.foodId = foodId;
            foodData.count = num;
            havingFoodData.havingFoodDictionary[foodId] = foodData;
        }
        else
        {
            havingFoodData.havingFoodDictionary[foodId].count += num;
        }
        LocalPrefs.Save(SaveData.Key.HavingFoodData, havingFoodData);
    }
}
