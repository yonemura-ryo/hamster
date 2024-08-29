public class InGameModel
{
    /// <summary>
    /// シーン遷移用
    /// </summary>
    private ISceneTransitioner sceneTransitioner;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="sceneTransitioner"></param>
    public InGameModel(ISceneTransitioner sceneTransitioner)
    {
        this.sceneTransitioner = sceneTransitioner;
    }

    /// <summary>
    /// TODO 一旦仮で用意しておく
    /// 次の画面へ
    /// </summary>
    public void NextScene()
    {
        sceneTransitioner.NextScene(SceneName.SCENE_SPLASH);
    }

    public void PreviousScene()
    {
        //
    }
}