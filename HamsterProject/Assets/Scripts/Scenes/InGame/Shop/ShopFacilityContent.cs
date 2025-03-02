using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class ShopFacilityContent : MonoBehaviour
{
    [SerializeField] Image facilityImage = null;
    [SerializeField] TextMeshProUGUI facilityNameText = null;
    [SerializeField] TextMeshProUGUI facilityLevelText = null;
    [SerializeField] TextMeshProUGUI facilityPriceText = null;
    [SerializeField] CustomButton levelUpButton = null;
    [SerializeField] CustomButton facilityDescriptionButton = null;
    private string FacilityDescription { get; set; }
    private bool isFacilityLevelMax = false;
    private int FacilityLevel { get; set; }
    private int FacilityPrice { get; set; }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="name"></param>
    /// <param name="facilityImagePath"></param>
    /// <param name="level"></param>
    /// <param name="price"></param>
    /// <param name="description"></param>
    /// <param name="isLevelMax"></param>
    /// <param name="onClickFacilityAction"></param>
    /// <param name="updateFacilityLevel"></param>
    /// <param name="confilmLevelMaxDialog"></param>
    public void Initialize(string name, string facilityImagePath, int level, int price, string description, bool isLevelMax, Action<string> onClickFacilityAction, Action<int, int, string> updateFacilityLevel, Action confilmLevelMaxDialog)
    {
        facilityNameText.text = name;
        facilityImage.sprite = Resources.Load<Sprite>(facilityImagePath);
        FacilityLevel = level;
        FacilityPrice = price;
        facilityLevelText.text = "Lv." + FacilityLevel;
        facilityPriceText.text = "¥" + FacilityPrice;
        FacilityDescription = description;
        isFacilityLevelMax = isLevelMax;
        facilityDescriptionButton.OnClickAsObservable().Subscribe(_ =>
        {
            onClickFacilityAction(FacilityDescription);
        });
        // 強化
        levelUpButton.OnClickAsObservable().Subscribe(_ =>
        {
            if (isFacilityLevelMax)
            {
                confilmLevelMaxDialog?.Invoke();
            }
            else
            {
                updateFacilityLevel?.Invoke(FacilityLevel, FacilityPrice, FacilityDescription);
            }
        });
    }

    /// <summary>
    /// 表示更新
    /// </summary>
    /// <param name="level"></param>
    /// <param name="price"></param>
    public void ResetContentView(int level, int price, string description, bool isLevelMax)
    {
        FacilityLevel = level;
        FacilityPrice = price;
        facilityLevelText.text = "Lv." + FacilityLevel;
        facilityPriceText.text = "¥" + FacilityPrice;
        FacilityDescription = description;
        isFacilityLevelMax = isLevelMax;
    }
}
