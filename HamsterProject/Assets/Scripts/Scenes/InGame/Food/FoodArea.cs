using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;

public class FoodArea : MonoBehaviour
{
    private int foodAreaId = 0;
    private Action<int> showFoodAreaDialog;
    /// <summary>
    /// 配置中の餌ID
    /// </summary>
    private int existFoodId = 0;
    /// <summary>
    /// 配置中餌の抽選ID
    /// </summary>
    private int existLotteryId = 0;
    
    [SerializeField] private Image foodAreaImage;
    [SerializeField] private Sprite emptyFoodSprite;
    [SerializeField] private Sprite fullFoodSprite;
    [SerializeField] private EventTrigger eventTrigger;
    public void Initialize(int foodAreaId, Action<int> showFoodAreaDialog)
    {
        this.foodAreaId = foodAreaId;
        this.showFoodAreaDialog = showFoodAreaDialog;

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener( (x) => OnClickFoodArea() );
        eventTrigger.triggers.Add(entry);
    }

    public int GetExistFoodId()
    {
        return existFoodId;
    }

    public int GetExitstLotteryId()
    {
        return existLotteryId;
    }
    
    /// <summary>
    /// 餌場クリック
    /// </summary>
    public void OnClickFoodArea()
    {
        // TODO 餌が配置済みの場合分岐
        showFoodAreaDialog(foodAreaId);
    }

    /// <summary>
    /// 餌配置
    /// </summary>
    public void SetFood(int foodId, int lotteryId)
    {
        existFoodId = foodId;
        existLotteryId = lotteryId;
        foodAreaImage.sprite = fullFoodSprite;
    }

    /// <summary>
    /// 餌が空
    /// </summary>
    public void SetEmptyFood()
    {
        existFoodId = 0;
        existLotteryId = 0;
        foodAreaImage.sprite = emptyFoodSprite;
    }
}