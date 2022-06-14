using UnityEngine;
using huacanacha.signal;
using huacanacha.unity.signal;

namespace huacanacha.signals.examples {

public class GameStateSignals : MonoBehaviour, ISignalProvider {
    public readonly CachedSignal<bool> sceneIsCurrentlyLoading = new CachedSignal<bool>();
    public readonly CachedSignal<SceneReference> sceneCurrentlyLoading = new CachedSignal<SceneReference>();
    /// Range [0,1]
    public readonly CachedSignal<float> sceneLoadingProgress = new CachedSignal<float>();

    public readonly ActionSignal activateInitialScene = new ActionSignal();
}

}