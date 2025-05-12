using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class BookDetailDialog : DialogBase
{
    [SerializeField] TextMeshProUGUI numberText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI rarityText;
    // TODO テキストじゃ無くなる
    [SerializeField] TextMeshProUGUI colorText;
    [SerializeField] TextMeshProUGUI lengthText;
    [SerializeField] TextMeshProUGUI characterText;
    [SerializeField] TextMeshProUGUI likeText;
    [SerializeField] TextMeshProUGUI capturedText;
    [SerializeField] TextMeshProUGUI featureText;
    [SerializeField] TextMeshProUGUI bornText;

    [SerializeField] Image hamsterImage;
    [SerializeField] Material shilhoetteMaterial;
    [SerializeField] Color nonCapturedHeaderColor;
    [SerializeField] Color nonCapturedBackgroundColor;
    [SerializeField] List<Image> headerImages;
    [SerializeField] List<Image> backgroundImages;

    public void Initialize(HamsterDetail hamsterDetail)
    {
        numberText.text = "No." + hamsterDetail.Id;
        if (hamsterDetail.IsCaptured)
        {
            //捕獲済み
            nameText.text = hamsterDetail.Name;
            rarityText.text = GetStarRating(hamsterDetail.Rarity);
            // TODO 色の仕様が決まったら修正
            colorText.text = hamsterDetail.ColorId.ToString();
            lengthText.text = hamsterDetail.LengthDetail;
            characterText.text = hamsterDetail.Character;
            likeText.text = hamsterDetail.Like;
            capturedText.text = hamsterDetail.CapturedCount.ToString();
            featureText.text = hamsterDetail.Feature;
            bornText.text = hamsterDetail.Born;

            hamsterImage.sprite = hamsterDetail.HamsterSprite;
        } else
        {
            nameText.text = "???";
            rarityText.text = "???";
            colorText.text = "?";
            lengthText.text = "???";
            characterText.text = "???";
            likeText.text = "???";
            capturedText.text = "0";
            // TODO 未捕獲のテキスト表示?
            featureText.text = "???";
            // TODO 未捕獲のテキスト表示?
            bornText.text = "???";
            // TODO 明確に固有の画像を設定するか共通で設定するか未確定
            hamsterImage.sprite = hamsterDetail.HamsterSprite;
            hamsterImage.material = shilhoetteMaterial;
            // ヘッダーと背景の色を変更
            foreach (Image headerImage in headerImages)
            {
                headerImage.color = nonCapturedHeaderColor;
            }

            foreach ( Image backgroundImage in backgroundImages)
            {
                backgroundImage.color = nonCapturedBackgroundColor;
            }
        }
    }

    public override void Show(Action closeAction = null)
    {
        base.Show(closeAction);
    }

    public override void Close()
    {
        base.Close();
    }

    public static string GetStarRating(int value)
    {
        int max = 5;
        value = Mathf.Clamp(value, 0, max); // 0～5に制限

        string stars = new string('★', value) + new string('☆', max - value);
        return stars;
    }
}


public class HamsterDetail
{
    public HamsterDetail(
        int id,
        bool isCaptured,
        string name,
        int rarity,
        int colorId,
        string lengthDetail,
        string character,
        string like,
        int capturedCount,
        string feature,
        string born,
        Sprite hamsterSprite
        )
    {
        Id = id;
        IsCaptured = isCaptured;
        Name = name;
        Rarity = rarity;
        ColorId = colorId;
        LengthDetail = lengthDetail;
        Character = character;
        Like = like;
        CapturedCount = capturedCount;
        Feature = feature;
        Born = born;
        HamsterSprite = hamsterSprite;
    }
    public int Id { get; }
    public bool IsCaptured { get; }
    public string Name { get; }
    public int Rarity { get; }
    public int ColorId { get; }
    public string LengthDetail { get; }
    public string Character { get; }
    public string Like { get; }
    public int CapturedCount { get; }
    public string Feature { get; }
    public string Born { get; }
    public Sprite HamsterSprite { get; }
}