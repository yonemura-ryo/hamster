using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class ShopFoodContent : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI foodNameText = null;
    [SerializeField] Image foodImage = null; 
    [SerializeField] TextMeshProUGUI foodPriceText = null; 
    [SerializeField] CustomButton foodDescriptionButton = null; 
    [SerializeField] CustomButton buyFoodButton = null; 
   
    public void Initialize(string foodName, string foodImagePath, int foodPrice, string foodDescription, Action<string>onClickFoodAction, Action onClickBuyAction)
    {
        // 商品名
        foodNameText.text = foodName;
        // 商品画像
        foodImage.sprite = Resources.Load<Sprite>(foodImagePath);
        // 商品の値段
        foodPriceText.text = "¥ " + foodPrice;
        // 商品説明表示
        foodDescriptionButton.OnClickAsObservable().Subscribe(_ =>
        {
            onClickFoodAction?.Invoke(foodDescription);
        }).AddTo(this);
        // 購入モーダル
        buyFoodButton.OnClickAsObservable().Subscribe(_ =>
        {
            onClickBuyAction?.Invoke();
        });
    }
}
