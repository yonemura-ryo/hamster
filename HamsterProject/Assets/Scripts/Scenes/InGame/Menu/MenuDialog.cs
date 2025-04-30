using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class MenuDialog : DialogBase
{
    [SerializeField] CustomButton BookButton  = null; // ?}??
    [SerializeField] CustomButton ShopButton  = null; // ?V???b?v
    [SerializeField] CustomButton MissionButton  = null; // ?~?b?V????
    [SerializeField] CustomButton HowToButton  = null; // ?V????
    [SerializeField] CustomButton SettingsButton  = null; // ????

    private IDialogContainer dialogContainer;
    private ISoundPlayer soundPlayer;
    private ISceneTransitioner sceneTransitioner;

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        BookButton.OnClickAsObservable().Subscribe(_ =>
        {

        }).AddTo(this);

        ShopButton.OnClickAsObservable().Subscribe(_ =>
        {

        }).AddTo(this);

        MissionButton.OnClickAsObservable().Subscribe(_ =>
        {

        }).AddTo(this);

        HowToButton.OnClickAsObservable().Subscribe(_ =>
        {

        }).AddTo(this);

        SettingsButton.OnClickAsObservable().Subscribe(_ =>
        {
            SettingDialog settingDialog = dialogContainer?.Show<SettingDialog>(OnCloseNextDialog);
            settingDialog?.Initialize(dialogContainer, soundPlayer, sceneTransitioner);
            gameObject.SetActive(false);
        }).AddTo(this);
    }

    /// <summary>
    /// ?_?C?A???O?\??
    /// </summary>
    public override void Show(Action closeAction = null)
    {
        base.Show(closeAction);
    }

    public void Initialize(IDialogContainer dialogContainer, ISoundPlayer soundPlayer, ISceneTransitioner sceneTransitioner)
    {
        this.dialogContainer = dialogContainer;
        this.soundPlayer = soundPlayer;
        this.sceneTransitioner = sceneTransitioner;
    }

    /// <summary>
    /// ?_?C?A???O????????
    /// </summary>
    public override void Close()
    {
        base.Close();
    }

    private void OnCloseNextDialog()
    {
        gameObject.SetActive(true);
    }
}
