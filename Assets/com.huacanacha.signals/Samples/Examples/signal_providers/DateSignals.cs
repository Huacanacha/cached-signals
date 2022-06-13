using UnityEngine;
using huacanacha.signal;
using huacanacha.unity.signal;

public class DateSignals : MonoBehaviour, ISignalProvider {
    readonly public CachedSignal<string> date = new CachedSignal<string>();
}
