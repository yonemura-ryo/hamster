using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �e�V�[���̃R���g���[���[��Base�N���X
/// </summary>
public abstract class SceneControllerBase : MonoBehaviour
{
    [SerializeField] private SceneBgmType sceneBgmType;

    private void Start()
    {
        Prepare();
        PrepareAsync().Forget();
        Initialize();
    }

    virtual protected async UniTask PrepareAsync() 
    {
        // return UniTask.CompletedTask;
    }

    /// <summary>
    /// ����(Async�Ȃǂ̑҂��������K�v�ł���ΑΉ����܂�)
    /// </summary>
    private void Prepare()
    {
        if (sceneBgmType == SceneBgmType.None) return;
        SystemScene.Instance.SoundPlayer.PlayBgm(SceneControllerDefine.SceneBgmTitle[(int)sceneBgmType]);
    }

    /// <summary>
    /// ������
    /// </summary>
    protected abstract void Initialize();
}
