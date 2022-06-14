using UnityEngine;
using huacanacha.signal;

namespace huacanacha.signals.examples {

public class DataCounterBinding : TextBinding<DataValueSignals, int, string> {
    protected override CachedSignal<int> GetSignal(DataValueSignals signalProvider) => signalProvider.counter;
}

}