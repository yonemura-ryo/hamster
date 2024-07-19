using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using UniRx;
using System;

/// <summary>
/// シーン遷移本体
/// </summary>
public class SceneTransitioner : MonoBehaviour, ISceneTransitioner
{
    [SerializeField] SceneFadeController sceneFadeController = null;

    /// <summary>
    /// シーン遷移時引き渡し用パラメータ
    /// </summary>
    public object SceneParam { private set; get; }

    /// <summary> 現在表示中のシーン名 </summary>
    private string nowSceneName = string.Empty;
    /// <summary> ロード後のシーン </summary>
    private Stack<Scene> loadedScenes = null;
    /// <summary> シーン遷移時に呼び出すコールバック </summary>
    private Action<bool> sceneTransitionCallBack = null;

    /// <summary>
    /// 初期化用
    /// </summary>
    public void Initialize(Action<bool> sceneTransitionCallBack)
    {
        this.sceneTransitionCallBack = sceneTransitionCallBack;

        loadedScenes = new Stack<Scene>();
        // 始めにシーン周りを保存しておく
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == SceneName.SCENE_SYSTEM) continue;
            nowSceneName = scene.name;
            loadedScenes.Push(scene);
        }

        sceneFadeController.Initialize();
    }

    /// <summary>
    /// 次のシーンへ遷移
    /// </summary>
    /// <param name="sceneName"></param>
    public void NextScene(string sceneName, object param = null, bool unloadPreviousScene = false)
    {
        if (sceneName == nowSceneName) return;

        Scene nowScene = SceneManager.GetSceneByName(nowSceneName);
        loadedScenes.Push(nowScene);

        nowSceneName = sceneName;
        SceneParam = param;

        sceneFadeController.FadeIn(async () =>
        {
            GameObject[] objects = nowScene.GetRootGameObjects();
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            // ロード完了次第前のシーンのオブジェクトを非アクティブに
            foreach (GameObject gameObject in objects)
            {
                gameObject.SetActive(false);
            }

            if (unloadPreviousScene)
            {
                foreach(Scene scene in loadedScenes)
                {
                    await SceneManager.UnloadSceneAsync(scene);
                }
                loadedScenes.Clear();
            }

            sceneTransitionCallBack?.Invoke(nowSceneName != SceneName.SCENE_IN_GAME);

            sceneFadeController.FadeOut();
        });
    }

    /// <summary>
    /// 後ろのシーンへ戻る
    /// </summary>
    public void BackScene()
    {
        sceneFadeController.FadeIn(async () =>
        {
            await SceneManager.UnloadSceneAsync(nowSceneName);
            Scene scene = loadedScenes.Pop();
            nowSceneName = scene.name;

            GameObject[] objects = scene.GetRootGameObjects();
            foreach (GameObject gameObject in objects)
            {
                gameObject.SetActive(true);
            }

            sceneTransitionCallBack?.Invoke(nowSceneName != SceneName.SCENE_IN_GAME);

            sceneFadeController.FadeOut();
        });
    }
}
