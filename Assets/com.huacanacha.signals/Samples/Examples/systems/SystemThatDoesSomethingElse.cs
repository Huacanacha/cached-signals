using UnityEngine;
using huacanacha.signal;

public class SystemThatDoesSomethingElse : MonoBehaviour {

    public class Signals {
        readonly public CachedSignal<int> frameCount = new CachedSignal<int>();
        readonly public CachedSignal<string> someValue = new CachedSignal<string>();
    }
    public readonly Signals signals = new Signals();

    void Start() {
        // I AM THE SystemThatDoesSomething, so tell everyone who cares ;)
        huacanacha.unity.signal.SignalDiscovery.
            GetSignalProvider<SystemsSignals>(this).systemThatDoesSomethingElse.Send(this);
    }

    void Update() {
        signals.frameCount.Send(Time.frameCount);
    }
}
