using UnityEngine;
using huacanacha.signal;
using huacanacha.unity.signal;

public class AppSystemSignals : MonoBehaviour, ISignalProvider {
    public readonly ReferenceSignal<Settings> settings = new ReferenceSignal<Settings>();
}
