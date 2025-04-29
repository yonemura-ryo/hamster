using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FoodAreaDialog : DialogBase
{
    [SerializeField] private GameObject foodContentPrefab;
    [SerializeField] private Transform scrollViewContentTransform;
    [SerializeField] private TextMeshProUGUI descriptionText;
    private int foodAreaId;
    private Action<int, int> parentUseFood;
    
    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="havingFoodDictionary"></param>
    /// <param name="parentUseFood"></param>
    public void Initialize(int foodAraaId, SerializableDictionary<int, FoodData> havingFoodDictionary, Action<int, int>parentUseFood)
    {
        descriptionText.text = "";
        IReadOnlyDictionary<int, FoodMaster> foodMasters = MasterData.DB.FoodMaster;
        this.foodAreaId = foodAraaId;
        this.parentUseFood = parentUseFood;
        // TODO 所持している餌を表示する
        foreach ((int key,FoodData havingFood) in havingFoodDictionary)
        {
            
            int id = havingFood.foodId;
            //Debug.Log(id + ":" +  havingFood.count);
            GameObject foodContentObject =  Instantiate(foodContentPrefab, Vector3.zero, Quaternion.identity, scrollViewContentTransform);
            foodContentObject.GetComponent<FoodContent>().Initialize(
                havingFood.foodId,
                $"2DAssets/Images/SpriteAtlasImages/item/item_{havingFood.foodId}",
                foodMasters[id].Name,
                havingFood.count,
                UseFood,
                () =>
                {
                    descriptionText.text = foodMasters[id].Description;
                }
                );
        }
    }

    /// <summary>
    /// 餌の使用
    /// </summary>
    /// <param name="foodId"></param>
    public void UseFood(int foodId)
    {
        Debug.Log("Use Food : " + foodId);
        // 餌の使用処理呼び出し
        parentUseFood(foodAreaId,foodId);
        Close();
    }
}