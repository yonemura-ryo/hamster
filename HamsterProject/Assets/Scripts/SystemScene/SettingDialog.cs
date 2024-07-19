using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

// システムシーン常駐の設定ダイアログ
public class SettingDialog : DialogBase
{
    [SerializeField] private Slider bgmSlider = null;
    [SerializeField] private Slider seSlider = null;
    [SerializeField] private Slider voiceSlider = null;
    [SerializeField] private Slider masterSlider = null;

    [SerializeField] private TextMeshProUGUI bgmVolumeText = null;
    [SerializeField] private TextMeshProUGUI seVolumeText = null;
    [SerializeField] private TextMeshProUGUI voiceVolumeText = null;
    [SerializeField] private TextMeshProUGUI masterVolumeText = null;

    /// <summary>
    /// ダイアログ表示
    /// </summary>
    public override void Show(Action closeAction)
    {
        base.Show(closeAction);
    }

    /// <summary>
    /// ダイアログを閉じる
    /// </summary>
    public override void Close()
    {
        base.Close();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="soundPlayer">サウンドプレイヤー</param>
    /// <param name="bgmVolume">BGMボリューム</param>
    /// <param name="seVolume">SEボリューム</param>
    /// <param name="voiceVolume">ボイスボリューム</param>
    public void Initialize(ISoundPlayer soundPlayer, SoundVolume soundVolume)
    {
        bgmSlider.value = soundVolume.BgmVolume;
        seSlider.value = soundVolume.SeVolume;
        voiceSlider.value = soundVolume.VoiceVolume;
        masterSlider.value = soundVolume.MasterVolume;

        bgmSlider.OnValueChangedAsObservable().Subscribe(volume =>
        {
            soundPlayer.SetBgmVolume(volume);
            bgmVolumeText.text = $"{Mathf.Floor(volume * 100)}";
        }).AddTo(this);

        seSlider.OnValueChangedAsObservable().Subscribe(volume =>
        {
            soundPlayer.SetSeVolume(volume);
            seVolumeText.text = $"{Mathf.Floor(volume * 100)}";
        }).AddTo(this);

        voiceSlider.OnValueChangedAsObservable().Subscribe(volume =>
        {
            soundPlayer.SetVoiceVolume(volume);
            voiceVolumeText.text = $"{Mathf.Floor(volume * 100)}";
        }).AddTo(this);

        masterSlider.OnValueChangedAsObservable().Subscribe(volume =>
        {
            soundPlayer.SetMasterVolume(volume);
            masterVolumeText.text = $"{Mathf.Floor(volume * 100)}";
        }).AddTo(this);
    }
}
