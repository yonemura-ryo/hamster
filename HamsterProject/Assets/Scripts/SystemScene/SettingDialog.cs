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
    [SerializeField] private Slider masterSlider = null;

    [SerializeField] private TextMeshProUGUI bgmVolumeText = null;
    [SerializeField] private TextMeshProUGUI seVolumeText = null;
    [SerializeField] private TextMeshProUGUI masterVolumeText = null;

    [SerializeField] private CustomButton privacyButton = null;
    [SerializeField] private CustomButton serviceButton = null;
    [SerializeField] private CustomButton deleteDataButton = null;

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
    /// <param name="sceneTransitioner">シーン遷移用</param>
    /// <param name="soundVolume">サウンドボリューム</param>
    public void Initialize(ISoundPlayer soundPlayer, ISceneTransitioner sceneTransitioner)
    {
        bgmSlider.value = soundPlayer.GetBgmVolume();
        seSlider.value = soundPlayer.GetSeVolume();
        masterSlider.value = soundPlayer.GetMasterVolume();

        bgmSlider.OnValueChangedAsObservable().Subscribe(volume =>
        {
            soundPlayer?.SetBgmVolume(volume);
            bgmVolumeText.text = $"{Mathf.Floor(volume * 100)}";
        }).AddTo(this);

        seSlider.OnValueChangedAsObservable().Subscribe(volume =>
        {
            soundPlayer?.SetSeVolume(volume);
            seVolumeText.text = $"{Mathf.Floor(volume * 100)}";
        }).AddTo(this);

        masterSlider.OnValueChangedAsObservable().Subscribe(volume =>
        {
            soundPlayer?.SetMasterVolume(volume);
            masterVolumeText.text = $"{Mathf.Floor(volume * 100)}";
        }).AddTo(this);

        privacyButton.OnClickAsObservable().Subscribe(_ =>
        {

        }).AddTo(this);

        serviceButton.OnClickAsObservable().Subscribe(_ =>
        {

        }).AddTo(this);

        deleteDataButton.OnClickAsObservable().Subscribe(_ =>
        {
            LocalPrefs.DeleteAll();
            sceneTransitioner.NextScene(SceneName.SCENE_SPLASH, null, true);
        }).AddTo(this);
    }
}
