using System.Collections;
using UnityEngine;
using System;
using UniRx;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class ShopDialog : DialogBase
{
    [SerializeField] GameObject shopFoodContentPrefab = null;
    [SerializeField] GameObject shopFacilityContentPrefab = null;
    [SerializeField] TextMeshProUGUI descriptionText = null;
    [SerializeField] TextMeshProUGUI userCoinText = null;
    [SerializeField] ScrollRect scrollRect = null;
    [SerializeField] RectTransform foodContent = null;
    [SerializeField] RectTransform facilityContent = null;
    [SerializeField] CustomButton foodTabButton = null;
    [SerializeField] CustomButton facilityTabButton = null;

    private IDialogContainer dialogContainer;
    private ISoundPlayer soundPlayer;
    private ISceneTransitioner sceneTransitioner;
    private UserCommonData userCommonData;
    private Action<int, int, int> buyFood = null;
    Func<int, int, bool> updateFacilityLevel = null;

    private IReadOnlyDictionary<string, FacilityLevelMaster> facilityLevelMaster = null;
    private IReadOnlyDictionary<int, FacilityMaster> facilityMasters = null;
    private void Start()
    {
        // 初期化
        descriptionText.text = "";
        foodContent.gameObject.SetActive(true);
        facilityContent.gameObject.SetActive(false);
        scrollRect.content = foodContent;
        // ボタンのリスナー登録
        foodTabButton.OnClickAsObservable().Subscribe(_ =>
        {
            foodContent.gameObject.SetActive(true);
            facilityContent.gameObject.SetActive(false);
            scrollRect.content = foodContent;
        });

        facilityTabButton.OnClickAsObservable().Subscribe(_ =>
        {
            facilityContent.gameObject.SetActive(true);
            foodContent.gameObject.SetActive(false);
            scrollRect.content = facilityContent;
        });
    }

    public void Initialize(
        IDialogContainer dialogContainer,
        ISoundPlayer soundPlayer,
        ISceneTransitioner sceneTransitioner,
        UserCommonData userCommonData,
        Action<int, int ,int>buyFood,
        FacilityListData facilityListData,
        Func<int, int, bool>updateFacilityLevel
        )
    {
        this.dialogContainer = dialogContainer;
        this.soundPlayer = soundPlayer;
        this.sceneTransitioner = sceneTransitioner;
        this.userCommonData = userCommonData;
        this.buyFood = buyFood;
        this.updateFacilityLevel = updateFacilityLevel;
        //userCoinText.text = userCommonData.coinCount.ToString();

        // マスタ読み込み
        facilityLevelMaster = MasterData.DB.FacilityLevelMaster;
        facilityMasters = MasterData.DB.FacilityMaster;

        // 餌のContentを作成する
        IReadOnlyDictionary<int, FoodMaster> foodMasters = MasterData.DB.FoodMaster;
        foreach ((int foodId, FoodMaster foodMaster) in foodMasters)
        {
            GameObject shopFoodContentObject = Instantiate(shopFoodContentPrefab, Vector3.zero, Quaternion.identity, foodContent);
            shopFoodContentObject.GetComponent<ShopFoodContent>().Initialize(
                foodMaster.Name,
                $"2DAssets/Images/SpriteAtlasImages/item/item_{foodMaster.FoodId}",
                foodMaster.Price,
                foodMaster.Description,
                (description) =>
                {
                    // 共通説明欄の文字列切り替え
                    descriptionText.text = description;
                },
                () => {
                    if(userCommonData.coinCount >= foodMaster.Price)
                    {
                        // 所持コインが足りているなら購入確認
                        OpenBuyFoodDialog(foodId, foodMaster.Name, 1, foodMaster.Price);
                    } else
                    {
                        // 不足しているので警告ダイアログ
                        OpenSinmpleConfilmDialog("コインが足りません");
                    }   
                }
                );
        }

        // 設備のContentを作成する
        foreach ((int facilityId, FacilityMaster facilityMaster) in facilityMasters)
        {
            int nextLevel = 1;
            int priceByLevel = 0;
            string description = facilityMaster.Description;
            bool isLevelMax = false;
            // 最大レベル取得
            FacilityLevelMaster maxLevelMaster = facilityLevelMaster
                .Where(x => x.Value.LevelType == facilityMaster.LevelType)
                .OrderByDescending(x => x.Value.Level)
                .FirstOrDefault()
                .Value
                ;
            // レベルと料金をセーブデータから推定
            if (facilityListData.facilityDictionary.ContainsKey(facilityId))
            {
                //
                FacilityData facilityData = facilityListData.facilityDictionary[facilityId];
                nextLevel = facilityData.level + 1;
                if(nextLevel > maxLevelMaster.Level)
                {
                    // 最大レベル到達済み
                    // TODO　最大レベルのため非アクティブ
                    nextLevel = maxLevelMaster.Level;
                    priceByLevel = 0;
                    description = replaceDescriptonText(description, maxLevelMaster.ReplaceText);
                    isLevelMax = true;
                }
                else
                {
                    // 該当レベルの強化コスト取得
                    FacilityLevelMaster nextLevelMaster = facilityLevelMaster
                        .Where(x => x.Value.LevelType == facilityMaster.LevelType)
                        .Where(x => x.Value.Level == nextLevel)
                        .FirstOrDefault()
                        .Value
                        ;
                    priceByLevel = nextLevelMaster.NeedCoin;
                    description = replaceDescriptonText(description, nextLevelMaster.ReplaceText);

                }
            }
            else
            {
                // 未解放
                FacilityLevelMaster nextLevelMaster = facilityLevelMaster
                        .Where(x => x.Value.LevelType == facilityMaster.LevelType)
                        .Where(x => x.Value.Level == 1)
                        .FirstOrDefault()
                        .Value
                        ;
                priceByLevel = nextLevelMaster.NeedCoin;
                description = replaceDescriptonText(description, nextLevelMaster.ReplaceText);
            }
            GameObject shopFacilityContentObject = Instantiate(shopFacilityContentPrefab, Vector3.zero, Quaternion.identity, facilityContent);
            ShopFacilityContent shopFacilityContent = shopFacilityContentObject.GetComponent<ShopFacilityContent>();
            shopFacilityContent.Initialize(
                facilityMaster.Name,
                $"2DAssets/Images/Facilities/{facilityMaster.ImagePath}",
                nextLevel,
                priceByLevel,
                description,
                isLevelMax,
                (facilityDescription) =>
                {
                    // レベル別文字列置換
                    descriptionText.text = facilityDescription;
                },
                (upLevel, upPriceByLevel, description) => {
                    if (userCommonData.coinCount >= upPriceByLevel)
                    {
                        var currentDescription = "なし";
                        if(upLevel > 1)
                        {
                            // 強化前レベルマスタ
                            FacilityLevelMaster currentLevelMaster = facilityLevelMaster
                            .Where(x => x.Value.LevelType == facilityMaster.LevelType)
                            .Where(x => x.Value.Level == upLevel - 1)
                            .FirstOrDefault()
                            .Value
                            ;
                            currentDescription = "Lv" + currentLevelMaster.Level + ":" + replaceDescriptonText(facilityMaster.Description, currentLevelMaster.ReplaceText);
                        }
                        
                        var confilmDescription =
                        "【" + facilityMaster.Name + "】"
                        + Environment.NewLine
                        + "を強化しますか？"
                        + Environment.NewLine + Environment.NewLine
                        + currentDescription
                        + Environment.NewLine
                        + "↓"
                        + Environment.NewLine
                        + "Lv" + upLevel + ":" + description
                        ;
                        // 購入確認ダイアログ
                        OpenLevelUpFacilityDialog(
                            confilmDescription,
                            facilityId,
                            upLevel,
                            upPriceByLevel,
                            maxLevelMaster,
                            shopFacilityContent
                            );
                    }
                    else
                    {
                        // 不足しているので警告ダイアログ
                        OpenSinmpleConfilmDialog("コインが足りません");
                    }
                },
                () => { OpenSinmpleConfilmDialog("レベルが最大です"); }
                );
        }

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
        // 所持コイン数を更新しておく
        //userCoinText.text = userCommonData.coinCount.ToString();
        // 詳細表示をリセットする
        descriptionText.text = "";
    }

    /// <summary>
    /// 施設強化
    /// </summary>
    /// <param name="facilityId"></param>
    /// <param name="upLevel"></param>
    /// <param name="upPriceByLevel"></param>
    /// <param name="maxLevelMaster"></param>
    /// <param name="shopFacilityContent"></param>
    private void LevelUpFacility(int facilityId, int upLevel, int upPriceByLevel, FacilityLevelMaster maxLevelMaster, ShopFacilityContent shopFacilityContent)
    {
        FacilityMaster facilityMaster = facilityMasters[facilityId];
        // 所持コインが足りているなら購入確認
        bool result = (bool)(updateFacilityLevel?.Invoke(facilityMaster.FacilityId, upPriceByLevel));
        if (result)
        {
            // レベルアップ成功
            if (upLevel >= maxLevelMaster.Level)
            {
                // 最大レベル
                shopFacilityContent.ResetContentView(
                    maxLevelMaster.Level,
                    0,
                    replaceDescriptonText(facilityMaster.Description, maxLevelMaster.ReplaceText),
                    true
                    );
            }
            else
            {
                FacilityLevelMaster nextLevelMaster = facilityLevelMaster
                    .Where(x => x.Value.LevelType == facilityMaster.LevelType)
                    .Where(x => x.Value.Level == (upLevel + 1))
                    .FirstOrDefault()
                    .Value
                    ;
                shopFacilityContent.ResetContentView(
                    nextLevelMaster.Level,
                    nextLevelMaster.NeedCoin,
                    replaceDescriptonText(facilityMaster.Description, nextLevelMaster.ReplaceText),
                    false
                    );
            }
        }
    }

    /// <summary>
    /// 購入確認ダイアログ
    /// </summary>
    /// <param name="foodId"></param>
    /// <param name="foodName"></param>
    /// <param name="count"></param>
    /// <param name="price"></param>
    private void OpenBuyFoodDialog(int foodId, string foodName, int count, int price)
    {
        ConfilmDialog confilmDialog = dialogContainer?.Show<ConfilmDialog>(OnCloseNextDialog);
        confilmDialog?.Initialize(
            "【" + foodName + "】" + Environment.NewLine + "をかいますか？",
            () => { buyFood(foodId, count, price); },
            null
            );
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 施設強化確認ダイアログ
    /// </summary>
    /// <param name="confilmMessage"></param>
    /// <param name="facilityId"></param>
    /// <param name="upLevel"></param>
    /// <param name="upPriceByLevel"></param>
    /// <param name="maxLevelMaster"></param>
    /// <param name="shopFacilityContent"></param>
    private void OpenLevelUpFacilityDialog(string confilmMessage, int facilityId, int upLevel, int upPriceByLevel, FacilityLevelMaster maxLevelMaster, ShopFacilityContent shopFacilityContent)
    {
        ConfilmDialog confilmDialog = dialogContainer?.Show<ConfilmDialog>(OnCloseNextDialog);
        confilmDialog?.Initialize(
            confilmMessage,
            () => {
                LevelUpFacility(
                    facilityId,
                    upLevel,
                    upPriceByLevel,
                    maxLevelMaster,
                    shopFacilityContent
                );
            },
            null
            );
        gameObject.SetActive(false);
    }

    /// <summary>
    /// シンプルな確認ダイアログ
    /// </summary>
    private void OpenSinmpleConfilmDialog(string message)
    {
        ConfilmDialog confilmDialog = dialogContainer?.Show<ConfilmDialog>(OnCloseNextDialog);
        confilmDialog?.Initialize(
            message,
            null,
            null
            );
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 文字列置換
    /// </summary>
    /// <param name="baseStr"></param>
    /// <param name="replaceStr"></param>
    /// <returns></returns>
    private string replaceDescriptonText(string baseStr, string replaceStr)
    {
        string targetStr = "{%}";
        baseStr = baseStr.Replace(targetStr, replaceStr);
        return baseStr;
    }
}
