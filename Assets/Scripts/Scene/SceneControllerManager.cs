using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControllerManager : SingletonMonoBehaviour<SceneControllerManager>
{
    private bool isFading;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private CanvasGroup faderCanvasGroup = null;
    [SerializeField] private Image faderImage = null;
    public SceneName startingSceneName;
    // 用于调整透明度的函数
    private IEnumerator Fade(float finalAlpha) {

        isFading = true;
        // 开始调整透明的时，阻止UI接受用户输入
        faderCanvasGroup.blocksRaycasts = true;
        // 计算调整的速度
        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeDuration;
        // 在多个游戏帧间调整透明度，可以看到变化的效果
        while (!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha)) {

            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);

            yield return null;
        }

        isFading = false;

        faderCanvasGroup.blocksRaycasts = false;
    }
    // 加载场景并将其设置为活动场景
    private IEnumerator LoadSceneAndSetActive(string sceneName) {
        // 通过场景管理器来异步加载场景，此处是逐步加载，sceneName是场景名字，加载模式是额外添加而不影响现有场景
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        // 获取加载的新场景，默认加载的场景放在场景列表的最后一个
        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        // 激活新场景
        SceneManager.SetActiveScene(newlyLoadedScene);
    } 
    // 开始调整透明度并切换场景
    private IEnumerator FadeAndSwitchScenes(string sceneName, Vector3 spawnPosition) {
        // 通知场景卸载并开始淡出的事件，有订阅者时可以接受此事件
        EventHandler.CallBeforeSceneUnloadFadeOutEvent();
        // 开始将透明度调整到1，即完全不透明，程序在此处暂停，直到Fade函数完成退出才继续运行下面的程序
        yield return StartCoroutine(Fade(1f));
        // 用于改变角色在场景中的位置
        Player.Instance.gameObject.transform.position = spawnPosition;
        // 通知场景卸载前已改变透明度事件
        EventHandler.CallBeforeSceneUnloadEvent();
        // 场景管理器开始卸载当前激活的场景，程序停在这里
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        // 开始加载新场景
        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));
        // 通知场景加载完事件
        EventHandler.CallAfterSceneLoadEvent();
        // 开始调整透明度到0，即完全透明
        yield return StartCoroutine(Fade(0f));
        // 通知场景加载完成并透明度调整完成
        EventHandler.CallAfterSceneLoadFadeInEvent();
    }
    // 脚本开始时调用一次，将画面设置为黑色，之后加载开始场景
    private IEnumerator Start() {

        faderImage.color = new Color(0f, 0f, 0f, 1f);
        faderCanvasGroup.alpha = 1f;
        // 开始加载开始场景
        yield return StartCoroutine(LoadSceneAndSetActive(startingSceneName.ToString()));
        // 通知场景加载完事件
        EventHandler.CallAfterSceneLoadEvent();
        // 开始将画面调整到透明
        StartCoroutine(Fade(0f));
    }
    // 调整透明度并加载新场景的函数，可由外部进行调用
    public void FadeAndLoadScene(string sceneName, Vector3 spawnPosition) {

        if (!isFading) {

            StartCoroutine(FadeAndSwitchScenes(sceneName, spawnPosition));
        }
    }

}
