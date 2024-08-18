using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameController : SceneControllerBase
{

    /// <summary>
    /// èâä˙âª
    /// </summary>
    protected override void Initialize()
    {
        SampleData data;
        data = LocalPrefs.Load<SampleData>("Sample");

        Debug.Log(data.text);
        Debug.Log(data.x.text);
    }
}
