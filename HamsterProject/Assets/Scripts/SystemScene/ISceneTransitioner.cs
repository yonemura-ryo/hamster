
/// <summary>
/// シーン遷移インターフェース
/// </summary>
public interface ISceneTransitioner
{
    /// <summary>
    /// 次のシーンへ
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    /// <param name="param">シーン遷移受け渡し用パラメータ</param>
    void NextScene(string sceneName, object param = null, bool unloadPreviousScenes = false);

    /// <summary>
    /// １つ前のシーンへ戻る
    /// </summary>
    void BackScene();
}
