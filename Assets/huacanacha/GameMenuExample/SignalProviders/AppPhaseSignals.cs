using UnityEngine;
using huacanacha.signal;
using huacanacha.unity.signal;

public enum AppPhase {Initializing, Menu, Game}

public class AppPhaseSignals : MonoBehaviour, ISignalProvider {
    public readonly CachedSignal<AppPhase> appPhase = new CachedSignal<AppPhase>();
}
