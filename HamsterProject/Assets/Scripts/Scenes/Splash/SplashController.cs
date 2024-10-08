using UnityEngine;

[System.Serializable]
public class SampleData
{
    public string text;
    public SampleDataData x;
}

[System.Serializable]
public class SampleDataData
{
    public string text;
}

/// <summary>
/// Splash Scene Controller.
/// </summary>
public class SplashController : SceneControllerBase
{
    [SerializeField] private SplashPresenter splashPresenter;

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Initialize()
    {
        SplashModel model  =new SplashModel(SystemScene.Instance.SceneTransitioner);
        splashPresenter.Initialize(model);

        SampleData Data = new SampleData();
        Data.text = "こんにちは";
        Data.x = new SampleDataData();
        Data.x.text = "こんばんは";

        LocalPrefs.Save(SaveData.Key.Sample, Data);
    }
}
