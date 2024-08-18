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
    /// èâä˙âª
    /// </summary>
    protected override void Initialize()
    {
        SplashModel model  =new SplashModel(SystemScene.Instance.SceneTransitioner);
        splashPresenter.Initialize(model);

        SampleData Data = new SampleData();
        Data.text = "Ç±ÇÒÇ…ÇøÇÕ";
        Data.x = new SampleDataData();
        Data.x.text = "Ç±ÇÒÇŒÇÒÇÕ";

        LocalPrefs.Save(SaveData.Key.Sample, Data);
    }
}
