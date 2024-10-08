/// <summary>
/// Splash Scene Model.
/// </summary>
public class SplashModel
{
    /// <summary> シーン遷移用 </summary>
    private ISceneTransitioner sceneTransitioner;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="sceneTransitioner"></param>
    public SplashModel(ISceneTransitioner sceneTransitioner)
    {
        this.sceneTransitioner = sceneTransitioner;
    }

    /// <summary>
    /// 次の画面へ
    /// </summary>
    public void NextScene()
    {
        sceneTransitioner.NextScene(SceneName.SCENE_IN_GAME);
    }
}
