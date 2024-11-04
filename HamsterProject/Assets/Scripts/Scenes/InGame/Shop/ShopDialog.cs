using System.Collections;
using UnityEngine;
using System;
using UniRx;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ShopDialog : DialogBase
{
    [SerializeField] GameObject shopFoodContentPrefab = null;
    [SerializeField] Transform scrollViewContentTransform = null;
    [SerializeField] TextMeshProUGUI discriptionText = null;
    [SerializeField] TextMeshProUGUI userCoinText = null;
    private IDialogContainer dialogContainer;
    private ISoundPlayer soundPlayer;
    private ISceneTransitioner sceneTransitioner;
    private UserCommonData userCommonData;
    private Action<int, int, int> buyFood;
    private void Start()
    {
        discriptionText.text = "";
        // TODO ボタンのリスナー登録
    }

    public void Initialize(
        IDialogContainer dialogContainer,
        ISoundPlayer soundPlayer,
        ISceneTransitioner sceneTransitioner,
        UserCommonData userCommonData,
        Action<int, int ,int>buyFood
        )
    {
        this.dialogContainer = dialogContainer;
        this.soundPlayer = soundPlayer;
        this.sceneTransitioner = sceneTransitioner;
        this.userCommonData = userCommonData;
        this.buyFood = buyFood;
        userCoinText.text = userCommonData.coinCount.ToString();

        // 餌のContentを作成する
        IReadOnlyDictionary<int, FoodMaster> foodMasters = MasterData.DB.FoodMaster;
        foreach ((int foodId, FoodMaster foodMaster) in foodMasters)
        {
            GameObject shopFoodContentObject = Instantiate(shopFoodContentPrefab, Vector3.zero, Quaternion.identity, scrollViewContentTransform);
            shopFoodContentObject.GetComponent<ShopFoodContent>().Initialize(
                foodMaster.Name,
                $"Images/Foods/{foodMaster.ImagePath}",
                foodMaster.Price,
                foodMaster.Description,
                (description) =>
                {
                    // 共通説明欄の文字列切り替え
                    discriptionText.text = description;
                },
                () => {
                    if(userCommonData.coinCount >= foodMaster.Price)
                    {
                        // 所持コインが足りているなら購入確認
                        OpenBuyFoodDialog(foodId, foodMaster.Name, 1, foodMaster.Price);
                    } else
                    {
                        // 不足しているので警告ダイアログ
                        OpenNotBuyFoodDialog();
                    }   
                }
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
        userCoinText.text = userCommonData.coinCount.ToString();
    }

    /// <summary>
    /// 購入確認モーダル
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
    /// コイン不足モーダル
    /// </summary>
    private void OpenNotBuyFoodDialog()
    {
        ConfilmDialog confilmDialog = dialogContainer?.Show<ConfilmDialog>(OnCloseNextDialog);
        confilmDialog?.Initialize(
            "コインが足りません",
            null,
            null
            );
        gameObject.SetActive(false);
    }
}
