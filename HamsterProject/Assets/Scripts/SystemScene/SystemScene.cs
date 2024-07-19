using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System.Linq;
using UniRx;

/// <summary>
/// Resident System Scene.
/// </summary>
public class SystemScene : SingletonMonobehaviour<SystemScene>
{
    [SerializeField] private SceneTransitionManager sceneTransitionManager = null;
    [SerializeField] private SoundManager soundManager = null;
    [SerializeField] private DialogContainer dialogContainer = null;

    protected override bool dontDestroyOnLoad { get { return true; } }

    private static GameObject[] childSceneObjects = null;

    /// <summary> サウンドプレイヤーインターフェース公開用 </summary>
    public ISoundPlayer SoundPlayer { get; private set; }

    /// <summary> シーン遷移インターフェース公開用</summary>
    public ISceneTransitioner SceneTransitioner { get; private set; }

    /// <summary>  ダイアログコンテナ公開用 </summary>
    public IDialogContainer DialogContainer => dialogContainer;

    /// <summary> シーン遷移引き渡しパラメータ </summary>
    public object SceneParam => sceneTransitionManager.SceneParam;

    #region SystemScenePrepare

    /// <summary>
    /// 起動時に実行する準備メソッド.
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialze()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if(sceneName != SceneName.SCENE_SYSTEM)
        {
            SetChildSceneActive(false);

            SceneManager.LoadSceneAsync(SceneName.SCENE_SYSTEM, LoadSceneMode.Additive);
        }
    }

    /// <summary>
    /// Unity System Awake().
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        Awaker().Forget();

        soundManager.Initialize(new SoundVolume(0.5f, 0.5f, 0.5f, 0.5f));
        sceneTransitionManager.Initialize(null);

        SoundPlayer = soundManager.GetSoundPlayer();
        SceneTransitioner = sceneTransitionManager.GetSceneTransitioner();
    }

    /// <summary>
    /// awake時の処理、非同期用
    /// </summary>
    /// <returns></returns>
    private async UniTask Awaker()
    {
        if(childSceneObjects != null)
        {
            SetChildSceneActive(true);
            childSceneObjects = null;
        }

        Scene systemScene = SceneManager.GetSceneByName(SceneName.SCENE_SYSTEM);

        await UniTask.WaitUntil(() => systemScene.isLoaded);
    }

    /// <summary>
    /// シーン内のオブジェクト全てのアクティブを切り替える
    /// </summary>
    /// <param name="value"></param>
    private static void SetChildSceneActive(bool value)
    {
        if (childSceneObjects == null)
        {
            childSceneObjects = FindObjectsOfType<Transform>()
                .Select(t => t.root.gameObject)
                .Distinct()
                .Where(gameObject => gameObject.activeInHierarchy)
                .ToArray();
        }

        foreach (GameObject gameObject in childSceneObjects)
        {
            gameObject.SetActive(value);
        }
    }

    #endregion
}
