using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameController : SceneControllerBase
{
    [SerializeField] private InGamePresenter inGamePresenter = null;
    [SerializeField] private HamsterManager hamsterManager = null;
    [SerializeField] private DialogContainer dialogContainer = null;
    
    /// <summary>  ダイアログコンテナ公開用 </summary>
    public IDialogContainer DialogContainer => dialogContainer;

    protected override UniTask PrepareAsync()
    {
        return UniTask.CompletedTask;
    }

    /// <summary>
    /// ������
    /// </summary>
    protected override void Initialize()
    {
        SampleData data;
        data = LocalPrefs.Load<SampleData>(SaveData.Key.Sample);

        Debug.Log(data.text);
        Debug.Log(data.x.text);

        InGameModel model = new InGameModel(SystemScene.Instance.SceneTransitioner);
        inGamePresenter.Initialize(model);

        hamsterManager.Initialize();
    }
}
