/// <summary>
/// Splash Scene Model.
/// </summary>
public class SplashModel
{
    /// <summary> ƒV[ƒ“‘JˆÚ—p </summary>
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
    /// Ÿ‚Ì‰æ–Ê‚Ö
    /// </summary>
    public void NextScene()
    {
        sceneTransitioner.NextScene(SceneName.SCENE_IN_GAME);
    }
}
