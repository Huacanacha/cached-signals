using UnityEngine;
using huacanacha.signal;
using huacanacha.unity.signal;

namespace huacanacha.signals.examples {

public class DataValueSignals : MonoBehaviour, ISignalProvider {
    readonly public CachedSignal<int> counter = new CachedSignal<int>();
    readonly public CachedSignal<string> text = new CachedSignal<string>();
}

}