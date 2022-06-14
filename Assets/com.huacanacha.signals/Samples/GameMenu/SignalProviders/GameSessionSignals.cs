using UnityEngine;
using huacanacha.signal;
using huacanacha.unity.signal;

namespace huacanacha.signals.examples {

public class GameSessionSignals : MonoBehaviour, ISignalProvider {
    public readonly ReferenceSignal<User> user = new ReferenceSignal<User>();
    public readonly CachedSignal<string> username = new CachedSignal<string>();
}

}