using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FoodAreaDialog : DialogBase
{
    public void Initialize(SerializableDictionary<int, FoodData> havingFoodDictionary)
    {
        // TODO 所持している餌を表示する
        foreach ((int key,FoodData havingFood) in havingFoodDictionary)
        {
            int id = havingFood.foodId;
            Debug.Log(id + ":" +  havingFood.count);
        }
    }
}