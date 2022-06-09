using System.Collections;
using System.Collections.Generic;
using huacanacha.signal;
using UnityEngine;

public class SceneLoadedBinding : ButtonInteractableBinding<GameStateSignals> {
    protected override CachedSignal<bool> GetSignal(GameStateSignals signalProvider) => signalProvider.sceneIsCurrentlyLoading;
}
