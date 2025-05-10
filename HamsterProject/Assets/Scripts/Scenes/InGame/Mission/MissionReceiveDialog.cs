using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MissionReceiveDialog : DialogBase
{
    [SerializeField] GameObject missionReceiveContentPrefab = null;
    [SerializeField] RectTransform content = null;

    public void Initialize(Dictionary<int,int> receiveItemList)
    {
        foreach (KeyValuePair<int, int> kvp in receiveItemList)
        {
            GameObject contentObject = Instantiate(missionReceiveContentPrefab, Vector3.zero, Quaternion.identity, content);
            contentObject.GetComponent<MissionReceiveContent>().Initialize(
                // TODO リソース仮（報酬次第で引っ張り方検討）
                Resources.Load<Sprite>($"2DAssets/Images/SpriteAtlasImages/item/item_{kvp.Key}"),
                kvp.Value
                );
        }
    }
}
