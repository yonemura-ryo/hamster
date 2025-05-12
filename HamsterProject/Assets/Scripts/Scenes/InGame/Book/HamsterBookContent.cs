using UnityEngine;
using System;
using UniRx;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class HamsterBookContent : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI numberText;
    [SerializeField] Image hamsterImage;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image headerImage;
    [SerializeField] Image backgroundImage;

    [SerializeField] Color nonCapturedHeaderColor;
    [SerializeField] Color nonCapturedBackGroundColor;
    [SerializeField] Material shilhoetteMaterial;
    [SerializeField] CustomButton detailButton;


    public void Initialize(HamsterDetail hamsterDetail, Action<HamsterDetail> openDetailDialog)
    {
        numberText.text = "No." + hamsterDetail.Id;
        if (hamsterDetail.IsCaptured)
        {
            //捕獲済み
            nameText.text = hamsterDetail.Name;
            hamsterImage.sprite = hamsterDetail.HamsterSprite;
        } else
        {
            // 未捕獲
            // TODO 明確に固有の画像を設定するか共通で設定するか未確定
            hamsterImage.sprite = hamsterDetail.HamsterSprite;
            hamsterImage.material = shilhoetteMaterial;
            headerImage.color = nonCapturedHeaderColor;
            backgroundImage.color = nonCapturedBackGroundColor;
            nameText.text = "???";
        }
        // 詳細ダイアログ
        detailButton.OnClickAsObservable().Subscribe(_ =>
        {
            openDetailDialog?.Invoke(hamsterDetail);
        });
    }
}
