using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class FoodArea : MonoBehaviour
{
    private int foodAreaId = 0;
    private Action<int> showFoodAreaDialog;
    public void Initialize(int foodAreaId, Action<int> showFoodAreaDialog)
    {
        this.foodAreaId = foodAreaId;
        this.showFoodAreaDialog = showFoodAreaDialog;
    }
    
    /// <summary>
    /// 餌場クリック
    /// </summary>
    public void OnClickFoodArea()
    {
        showFoodAreaDialog(foodAreaId);
    }
}