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
    [SerializeField] private TextMeshProUGUI foodName;
    [SerializeField] private TextMeshProUGUI num;
    [SerializeField] private CustomButton useButton;
    [SerializeField] private CustomButton selectButton;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="foodId"></param>
    /// <param name="foodImagePath"></param>
    /// <param name="foodName"></param>
    /// <param name="num"></param>
    /// <param name="onClickUse"></param>
    public void Initialize(int foodId, string foodImagePath, string foodName, int num, Action<int> onClickUse, Action onClickFood)
    {
        this.foodImage.sprite = Resources.Load<Sprite>(foodImagePath);
        this.foodName.text = foodName;
        this.num.text = num.ToString();
        useButton.onClick.AddListener(() => onClickUse(foodId));
        selectButton.onClick.AddListener(() => onClickFood());
    }
}