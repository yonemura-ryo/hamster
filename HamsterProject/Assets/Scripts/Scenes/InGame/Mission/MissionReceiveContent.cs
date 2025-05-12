using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class MissionReceiveContent : MonoBehaviour
{
    [SerializeField] Image rewardImage = null;
    [SerializeField] TextMeshProUGUI rewardText = null;

    public void Initialize(Sprite rewardSprite, int rewardCount)
    {
        rewardImage.sprite = rewardSprite;
        rewardText.text = "x" + rewardCount.ToString();
    }
}
