using UnityEngine;
using huacanacha.signal;
using huacanacha.unity.signal;

namespace huacanacha.signals.examples {

public class TimeSignals : MonoBehaviour, ISignalProvider {
    readonly public CachedSignal<string> time = new CachedSignal<string>();
}

}