using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Facility : MonoBehaviour
{
    private int facilityId = 0;
    private string facilityName = "";
    private int facilityLevel = 0;
    private Action<int> onClickFacilityAction;
    [SerializeField] Sprite facilitySprite;

    public Sprite FacilitySprite
    {
        get { return facilitySprite; }
    }
    
    public void Initialize(int id, string name, int level, Action<int> onClickFacilityAction)
    {
        facilityId = id;
        facilityName = name;
        facilityLevel = level;
        this.onClickFacilityAction = onClickFacilityAction;
    }

    public void OnClickFacility()
    {
        // TODO ダイアログを出して施設レベルアップ
        Debug.Log(facilityId + ":" + facilityName + ":" + facilityLevel);
        onClickFacilityAction(facilityId);
    }

    public void UpdateMemberValue(int level)
    {
        facilityLevel = level;
    }
}