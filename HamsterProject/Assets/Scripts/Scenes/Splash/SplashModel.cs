/// <summary>
/// Splash Scene Model.
/// </summary>
public class SplashModel
{
    /// <summary> �V�[���J�ڗp </summary>
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
    /// ���̉�ʂ�
    /// </summary>
    public void NextScene()
    {
        sceneTransitioner.NextScene(SceneName.SCENE_IN_GAME);
    }
}
