using UnityEngine;
using huacanacha.signal;

namespace huacanacha.signals.examples {

public class TimeBinding : TextBinding<TimeSignals, string, string> {
    protected override CachedSignal<string> GetSignal(TimeSignals signalProvider) => signalProvider.time;
}

}