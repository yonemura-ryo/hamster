
/// <summary>
/// Sound Player Interface
/// </summary>
public interface ISoundPlayer
{
    #region Play Sound

    /// <summary>
    /// Play SE.
    /// </summary>
    /// <param name="soundName">sound name SE.</param>
    void PlaySe(string soundName);

    /// <summary>
    /// Play BGM.
    /// </summary>
    /// <param name="soundName">sound name BGM.</param>
    void PlayBgm(string soundName);

    /// <summary>
    /// Play Voice.
    /// </summary>
    /// <param name="soundName">sound name voice.</param>
    void PlayVoice(string soundName);

    #endregion

    #region Set Volume.

    /// <summary>
    /// Set volume SE.
    /// </summary>
    /// <param name="volume">volume</param>
    void SetSeVolume(float volume);

    /// <summary>
    /// Set volume BGM.
    /// </summary>
    /// <param name="volume">volume</param>
    void SetBgmVolume(float volume);

    /// <summary>
    /// Set volume voice.
    /// </summary>
    /// <param name="volume">volume</param>
    void SetVoiceVolume(float volume);

    /// <summary>
    /// マスタ音量設定
    /// </summary>
    /// <param name="masterVolume"></param>
    void SetMasterVolume(float volume);

    #endregion

    #region Stop Sound

    /// <summary>
    /// Stop SE.
    /// </summary>
    void StopSe();

    /// <summary>
    /// Stop BGM.
    /// </summary>
    void StopBgm();

    /// <summary>
    /// Stop Voice.
    /// </summary>
    void StopVoice();

    #endregion

    #region Pause Sound

    /// <summary>
    /// Pause SE.
    /// </summary>
    void PauseSe();

    /// <summary>
    /// Pause BGM.
    /// </summary>
    void PauseBgm();

    /// <summary>
    /// Pause Voice.
    /// </summary>
    void PauseVoice();

    #endregion

    #region UnPause Sound

    /// <summary>
    /// UnPause SE.
    /// </summary>
    void UnPauseSe();

    /// <summary>
    /// UnPause BGM.
    /// </summary>
    void UnPauseBgm();

    /// <summary>
    /// UnPause Voice.
    /// </summary>
    void UnPauseVoice();

    #endregion

    #region Volume Getter

    /// <summary>
    /// マスターボリューム取得
    /// </summary>
    /// <returns></returns>
    float GetMasterVolume();

    /// <summary>
    /// BGM音量取得
    /// </summary>
    /// <returns></returns>
    float GetBgmVolume();

    /// <summary>
    /// SE音量取得
    /// </summary>
    /// <returns></returns>
    float GetSeVolume();

    /// <summary>
    /// ボイス音量取得
    /// </summary>
    /// <returns></returns>
    float GetVoiceVolume();

    #endregion
}
