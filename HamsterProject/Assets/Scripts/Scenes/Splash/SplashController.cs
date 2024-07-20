using UnityEngine;

/// <summary>
/// Splash Scene Controller.
/// </summary>
public class SplashController : SceneControllerBase
{
    [SerializeField] private SplashPresenter splashPresenter;

    /// <summary>
    /// ������
    /// </summary>
    protected override void Initialize()
    {
        SplashModel model  =new SplashModel(SystemScene.Instance.SceneTransitioner);
        splashPresenter.Initialize(model);
    }
}
