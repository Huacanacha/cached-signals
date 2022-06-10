using UnityEngine;
using huacanacha.signal;
using huacanacha.unity.signal;

public class CantFindMeSignals : MonoBehaviour, ISignalProvider {
    readonly public CachedSignal<string> imHiding = new CachedSignal<string>();
}
