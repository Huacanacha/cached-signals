using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using huacanacha.signal;
using huacanacha.unity.signal;

public class SceneLoaderX : MonoBehaviour {

    public class Signals {
        /// Range [0,1]
        // readonly public CachedSignal<float> loadingProgress = new CachedSignal<float>();
        // readonly public CachedSignal loadingComplete = new CachedSignal();
    }
    public readonly Signals signals = new Signals();

    // public SceneReference bootstrapScene;
    public SceneReference initialScene;
    // public TMPro.TMP_Text text;
    bool loadImmediately = false;

    private GameStateSignals _gameStateSignals;

    void Start() {
        _gameStateSignals = SignalDiscovery.GetSignalProvider<GameStateSignals>(this);
        if (_gameStateSignals == null) {
            Debug.LogError("GameStateSignals not found, and therefore we simply cannot proceed :(");
            Destroy(gameObject);
            return;
        }
        // _signals.bootstrapLoaded.ClearCache();
        StartCoroutine(LoadScene(initialScene));
        _gameStateSignals.activateInitialScene.SubscribeOnce(() => loadImmediately = true);
        _gameStateSignals.sceneCurrentlyLoading.Subscribe(OnSceneCurrentlyLoading);
    }

    IEnumerator LoadScene(SceneReference scene) {
        yield return null;
        _gameStateSignals.sceneCurrentlyLoading.Send(scene);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.ScenePath, LoadSceneMode.Additive);

        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f) {
            _gameStateSignals.sceneLoadingProgress.Send(asyncLoad.progress);
            // text.text = $"<b>Loading progress {asyncLoad.progress * 100}%</b>\n ";
            // yield return null;
            yield return new WaitForSecondsRealtime(1f);
        }
        Debug.Log($"Loaded scene: ${scene.ScenePath}\nWaiting for activation.");
        // text.text = "<b>Loading progress 100%</b>\n\n<size=70%>Click or press space to continue...";
        _gameStateSignals.sceneLoadingProgress.Send(1);

        while (!Input.GetKeyDown(KeyCode.Space) && !loadImmediately) {
            yield return null;
        }
        Debug.Log($"Activating scene: ${scene.ScenePath}");
        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone) {
            yield return null;
        }
        Debug.Log($"FINISHED Activating scene: ${scene.ScenePath}");

        // signals.loadingComplete.Send();
        _gameStateSignals.sceneCurrentlyLoading.Send(null);

        Destroy(gameObject);
    }

    void OnSceneCurrentlyLoading(SceneReference scene) {
        Debug.Log($"Sending: sceneIsCurrentlyLoading = {scene != null}");
        _gameStateSignals.sceneIsCurrentlyLoading.Send(scene != null);
    }
}