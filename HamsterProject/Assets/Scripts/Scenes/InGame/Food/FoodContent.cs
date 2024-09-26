using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class FoodContent : MonoBehaviour
{
    [SerializeField] Image foodImage;
    [SerializeField] private Text foodName;
    [SerializeField] private Text num;
    [SerializeField] private Button useButton;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="foodId"></param>
    /// <param name="foodImagePath"></param>
    /// <param name="foodName"></param>
    /// <param name="num"></param>
    /// <param name="onClickUse"></param>
    public void Initialize(int foodId, string foodImagePath, string foodName, int num, Action<int> onClickUse)
    {
        this.foodImage.sprite = Resources.Load<Sprite>(foodImagePath);
        this.foodName.text = foodName;
        this.num.text = num.ToString();
        useButton.onClick.AddListener(() => onClickUse(foodId));
    }
}