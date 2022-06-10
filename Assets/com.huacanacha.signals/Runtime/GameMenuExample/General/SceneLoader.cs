using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {  

    public SceneReference sceneToLoad;
    public LoadSceneMode mode = LoadSceneMode.Additive;

    public void LoadScene() {
        StartCoroutine(LoadScene(sceneToLoad));
    }

    IEnumerator LoadScene(SceneReference scene) {
        yield return null;
        // Optional -> send scene currently loading (via signals or other mechanism)
        // _gameStateSignals.sceneCurrentlyLoading.Send(scene);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.ScenePath, mode);

        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f) {
            // Optional -> send loading progress (via signals or other mechanism)
            // _gameStateSignals.sceneLoadingProgress.Send(asyncLoad.progress);
            yield return new WaitForSecondsRealtime(1f);
        }
        Debug.Log($"Loaded scene: ${scene.ScenePath}\nWaiting for activation.");
        // Optional -> send loading progress 100% (via signals or other mechanism)
        // _gameStateSignals.sceneLoadingProgress.Send(1);

        // Optional -> wait for user input or signal etc before activating scene?
        // while (!Input.GetKeyDown(KeyCode.Space) && !loadImmediately) {
        //     yield return null;
        // }
        Debug.Log($"Activating scene: ${scene.ScenePath}");
        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone) {
            yield return null;
        }
        Debug.Log($"FINISHED Activating scene: ${scene.ScenePath}");

        // Optional -> clear 'scene currently loading' (via signals or other mechanism)
        // _gameStateSignals.sceneCurrentlyLoading.Send(null);

        Destroy(gameObject);
    }
}