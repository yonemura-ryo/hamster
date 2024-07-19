using UnityEngine;

/// <summary>
/// Sound Presenter.
/// </summary>
public class SoundPresenter : MonoBehaviour, ISoundPlayer
{
    [SerializeField] private AudioSource seAudio;
    [SerializeField] private AudioSource bgmAudio;
    [SerializeField] private AudioSource voiceAudio;

    private SoundModel soundModel = null;

    /// <summary>
    /// Initialize Sound
    /// </summary>
    public void Initialize(SoundModel model)
    {
        soundModel = model;

        seAudio.loop = false;
        bgmAudio.loop = true;
        voiceAudio.loop = false;

        SetMasterVolume(soundModel.MasterVolume);
    }

    /// <summary>
    /// getter sound volume.
    /// </summary>
    /// <returns></returns>
    public SoundVolume GetSoundVolume()
    {
        return soundModel.GetSoundvolume();
    }

    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="soundName"></param>
    public void PlaySe(string soundName)
    {
        AudioClip se = Resources.Load<AudioClip>($"Sound/Se/{soundName}");
        seAudio.PlayOneShot(se);
    }

    /// <summary>
    /// BGM再生
    /// </summary>
    /// <param name="soundName"></param>
    public void PlayBgm(string soundName)
    {
        if (bgmAudio.clip?.name == soundName) return;

        bgmAudio.Stop();
        AudioClip bgm = Resources.Load<AudioClip>($"Sound/Bgm/{soundName}");
        bgmAudio.clip = bgm;
        bgmAudio.Play();
    }

    /// <summary>
    /// VOICE再生
    /// </summary>
    /// <param name="soundName"></param>
    public void PlayVoice(string soundName)
    {
        AudioClip voice = Resources.Load<AudioClip>($"Sound/Voice/{soundName}");
        voiceAudio.PlayOneShot(voice);
    }

    public void SetSeVolume(float volume)
    {
        seAudio.volume = soundModel.CalcSeVolume(volume);
    }

    public void SetBgmVolume(float volume)
    {
        bgmAudio.volume = soundModel.CalcBgmVolume(volume);
    }

    public void SetVoiceVolume(float volume)
    {
        voiceAudio.volume = soundModel.CalcVoiceVolume(volume);
    }


    public void StopSe()
    {
        seAudio.Stop();
    }

   public  void StopBgm()
    {
        bgmAudio.Stop();
    }

    public void StopVoice()
    {
        voiceAudio.Stop();
    }

    public void PauseSe()
    {
        seAudio.Pause();
    }

    public void PauseBgm()
    {
        bgmAudio.Pause();
    }

    public void PauseVoice()
    {
        voiceAudio.Pause();
    }


    public void UnPauseSe()
    {
        seAudio.UnPause();
    }

    public void UnPauseBgm()
    {
        bgmAudio.UnPause();
    }

    public void UnPauseVoice()
    {
        voiceAudio.UnPause();
    }

    public void SetMasterVolume(float volume)
    {
        soundModel.SetMasterVolume(volume);
        bgmAudio.volume = soundModel.CalcBgmVolume(soundModel.BgmVolume);
        seAudio.volume = soundModel.CalcSeVolume(soundModel.SeVolume);
        voiceAudio.volume = soundModel.CalcVoiceVolume(soundModel.VoiceVolume);
    }
}
