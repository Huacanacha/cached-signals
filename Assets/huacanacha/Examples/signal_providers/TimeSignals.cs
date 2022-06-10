using UnityEngine;
using huacanacha.signal;
using huacanacha.unity.signal;

public class TimeSignals : MonoBehaviour, ISignalProvider {
    readonly public CachedSignal<string> time = new CachedSignal<string>();
}
