using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class MenuDialog : DialogBase
{
    [SerializeField] CustomButton BookButton  = null; // �}��
    [SerializeField] CustomButton ShopButton  = null; // �V���b�v
    [SerializeField] CustomButton EnhanceButton  = null; // ����
    [SerializeField] CustomButton MissionButton  = null; // �~�b�V����
    [SerializeField] CustomButton HowToButton  = null; // �V�ѕ�
    [SerializeField] CustomButton SettingsButton  = null; // �ݒ�

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

        EnhanceButton.OnClickAsObservable().Subscribe(_ =>
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

        }).AddTo(this);
    }

    /// <summary>
    /// �_�C�A���O�\��
    /// </summary>
    public override void Show(Action closeAction = null)
    {
        base.Show(closeAction);
    }

    /// <summary>
    /// �_�C�A���O�����
    /// </summary>
    public override void Close()
    {
        base.Close();
    }
}
