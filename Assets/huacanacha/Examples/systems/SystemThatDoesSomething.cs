using UnityEngine;
using huacanacha.signal;

public class SystemThatDoesSomething : MonoBehaviour {

    public class Signals {
        readonly public CachedSignal<int> frameCount = new CachedSignal<int>();
        readonly public CachedSignal<string> someValue = new CachedSignal<string>();
    }
    public readonly Signals signals = new Signals();

    void Start() {
        // I AM THE SystemThatDoesSomething, so tell everyone who cares ;)
        var signallingContect = huacanacha.unity.signal.SignalDiscovery.
            GetSignalProvider<SystemsSignals>(this);
        signallingContect.systemThatDoesSomething.Send(this);
    }

    void Update() {
        signals.frameCount.Send(Time.frameCount);
    }
}
