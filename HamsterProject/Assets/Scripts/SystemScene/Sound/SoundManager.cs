using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sound Manager.
/// </summary>
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundPresenter soundPresenter;

    /// <summary>
    /// initialize sound manager.
    /// </summary>
    public void Initialize(SoundVolume volume)
    {
        if(soundPresenter == null)
        {
            return;
        }

        SoundModel model = new SoundModel(volume);
        soundPresenter.Initialize(model);
    }

    /// <summary>
    /// インターフェース公開用
    /// </summary>
    /// <returns></returns>
    public ISoundPlayer GetSoundPlayer()
    {
        return soundPresenter;
    }

    /// <summary>
    /// getter sound volume.
    /// </summary>
    /// <returns></returns>
    public SoundVolume GetSoundVolume()
    {
        return soundPresenter.GetSoundVolume();
    }
}

/// <summary>
/// Pass value to sound. 
/// </summary>
public class SoundVolume
{
    public float MasterVolume { private set; get; }
    public float BgmVolume { private set; get; }
    public float SeVolume { private set; get; }
    public float VoiceVolume { private set; get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="masterVolume"></param>
    /// <param name="bgmVolume"></param>
    /// <param name="seVolume"></param>
    /// <param name="voiceVolume"></param>
    public SoundVolume(float masterVolume, float bgmVolume, float seVolume, float voiceVolume)
    {
        MasterVolume = masterVolume;
        BgmVolume = bgmVolume;
        SeVolume = seVolume;
        VoiceVolume = voiceVolume;
    }
}