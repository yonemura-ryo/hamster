using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FacilityLevelUpDialog : DialogBase
{
    private int facilityId = 0;
    private string facilityName = "";
    private int facilityLevelType = 0;
    private int facilityLevel = 0;
    private IReadOnlyDictionary<string, FacilityLevelMaster> FacilityLevelMaster = null;
    private Func<int,int,bool> updateFacilityLevel = null;
    
    [SerializeField] private Text facilityNameText;
    [SerializeField] private Text facilityLevelText;
    [SerializeField] private Text facilityLevelUpButtonText;
    [SerializeField] private Text facilityNeedCoinText;
    [SerializeField] private Image facilityImage;
    
    public void Initialize(int id, string name, int levelType, int level, Sprite facilitySprite, Func<int, int, bool> updateFacilityLevel)
    {
        facilityId = id;
        facilityLevelType = levelType;
        facilityLevel = level;
        this.updateFacilityLevel = updateFacilityLevel;
        facilityImage.sprite = facilitySprite;
        
        facilityNameText.text = name;
        if (level == 0)
        {
            facilityLevelUpButtonText.text = "解放";
        }
        // レベルマスタ
        FacilityLevelMaster = MasterData.DB.FacilityLevelMaster;
        UpdateView();
    }

    public void OnClickLevelUpButton()
    {
        if(IsLevelMax()){ return; }
        // コインの消費, 施設のレベルアップ
        var result = updateFacilityLevel(facilityId, FacilityLevelMaster[MakeFacilityLevelMasterIndex(facilityLevelType, facilityLevel)].NeedCoin);
        if (result == false)
        {
            // TODO コイン不足ダイアログ
            return;
        }
        // TODO レベルアップ演出
        facilityLevel++;
        UpdateView();
    }

    private void UpdateView()
    {
        if (IsLevelMax())
        {
            // 最大レベル
            facilityLevelText.text = "Lv.Max";
            facilityNeedCoinText.text = "-";
            facilityLevelUpButtonText.text = "最大レベルです";
        }
        else
        {
            // 現在のレベル
            facilityLevelText.text = "Lv." + facilityLevel.ToString();
            // レベルアップに必要なコイン数
            facilityNeedCoinText.text = FacilityLevelMaster[MakeFacilityLevelMasterIndex(facilityLevelType, facilityLevel)].NeedCoin.ToString();
            // レベルアップボタン
            facilityLevelUpButtonText.text = "レベルアップ";
        }
    }

    /// <summary>
    /// レベルマスタのインデックス文字列を生成
    /// </summary>
    /// <param name="levelType"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    private string MakeFacilityLevelMasterIndex(int levelType, int level)
    {
        return levelType + ":" + (level + 1);
    }
    
    /// <summary>
    /// レベル上限判定
    /// </summary>
    /// <returns></returns>
    private bool IsLevelMax()
    {
        return !FacilityLevelMaster.ContainsKey(MakeFacilityLevelMasterIndex(facilityLevelType, facilityLevel + 1));
    }
}