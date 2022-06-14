using UnityEngine;
using huacanacha.signal;

public class TimeBinding : TextBinding<TimeSignals, string, string> {
    protected override CachedSignal<string> GetSignal(TimeSignals signalProvider) => signalProvider.time;
}
