using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class MissionContent : MonoBehaviour
{
    [SerializeField] Image rewardImage = null;
    [SerializeField] TextMeshProUGUI rewardText = null;
    [SerializeField] TextMeshProUGUI detailText = null;
    [SerializeField] TextMeshProUGUI progressText = null;
    [SerializeField] Image progressImage = null;
    [SerializeField] CustomButton receiveButton = null;
    [SerializeField] GameObject receivedMask = null;
    [SerializeField] TextMeshProUGUI receiveButtonText = null;
    [SerializeField] Image receiveButtonImage = null;
    [SerializeField] Sprite receiveButtonSprite = null;
    [SerializeField] Color receiveTextColor = Color.white;

    public void Initialize(
        int missionId,
        Sprite rewardSprite,
        int rewardCount,
        string detailStr,
        int completeCount,
        int currentCount,
        bool isCompleted,
        bool isReceived,
        Action<int> receiveAction
        )
    {
        rewardImage.sprite = rewardSprite;
        rewardText.text = "x" + rewardCount.ToString();
        detailText.text = missionTextReplace(detailStr, completeCount);
        progressText.text = currentCount.ToString() + "/" + completeCount.ToString();
        progressImage.fillAmount = (float)currentCount / (float)completeCount;
        if (isReceived)
        {
            // 受取済み
            receivedMask.SetActive(true);
            // TODO 受取済み時の設定(デフォルトで設定はしているが)
        } else
        {
            // 受取済みマスクを外す
            receivedMask.SetActive(false);
            // ボタンテキスト黄色にする
            receiveButtonText.color = receiveTextColor;
            if (isCompleted)
            {
                // 受取可能状態
                // 座標とSpriteを変える
                receiveButtonImage.sprite = receiveButtonSprite;
                receiveButtonImage.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                receiveButton.OnClickAsObservable().Subscribe(_ => {
                    receiveAction?.Invoke(missionId);
                });
                receiveButtonText.text = "受取";
            } else
            {
                receiveButtonText.text = "遂行中";
            }
        }
    }

    /// <summary>
    /// ミッション文の変数値を置換かける
    /// </summary>
    /// <param name="masterText"></param>
    /// <param name="completeCount"></param>
    /// <returns></returns>
    private string missionTextReplace(string masterText, int completeCount)
    {
        return masterText.Replace("{%}", completeCount.ToString());
    }
}
