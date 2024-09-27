using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sound Model.
/// </summary>
public class SoundModel
{
    public float MasterVolume { get; private set; }
    public float BgmVolume { get;private set; }
    public float SeVolume { get; private set; }
    //public float VoiceVolume { get; private set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="volume"></param>
    public SoundModel(SoundVolume volume)
    {
        MasterVolume = volume.MasterVolume;
        BgmVolume = volume.BgmVolume;
        SeVolume = volume.SeVolume;
        //VoiceVolume = volume.VoiceVolume;
    }

    /// <summary>
    /// サウンドの値が0~1に留まるように
    /// </summary>
    /// <param name="volume"></param>
    /// <returns></returns>
    private float CorrectionVolume(float volume)
    {
        return Mathf.Clamp(volume, 0, 1);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="volume"></param>
    /// <returns></returns>
    public float CalcBgmVolume(float volume)
    {
        BgmVolume = CorrectionVolume(volume);
        return BgmVolume * SoundDefine.BgmCorrectionValue * MasterVolume;
    }

    /// <summary>
    /// SEの音量計算
    /// </summary>
    /// <param name="volume"></param>
    /// <param name="correctionValue"></param>
    /// <returns></returns>
    public float CalcSeVolume(float volume)
    {
        SeVolume = CorrectionVolume(volume);
        return SeVolume * SoundDefine.SeCorrectionValue * MasterVolume;
    }

    /// <summary>
    /// ボイス音量計算
    /// </summary>
    /// <param name="volume"></param>
    /// <returns></returns>
    public float CalcVoiceVolume(float volume)
    {
        //VoiceVolume = CorrectionVolume(volume);
        return 0.0f; // VoiceVolume * SoundDefine.VoiceCorrenctionValue * MasterVolume;
    }

    /// <summary>
    /// マスター音量計算
    /// </summary>
    /// <param name="volume"></param>
    /// <returns></returns>
    public void SetMasterVolume(float volume)
    {
        MasterVolume = CorrectionVolume(volume);
    }

    /// <summary>
    /// サウンドボリュームゲッター
    /// </summary>
    /// <returns></returns>
    public SoundVolume GetSoundvolume()
    {
        return new SoundVolume(MasterVolume, BgmVolume, SeVolume/*, VoiceVolume*/);
    }
}
