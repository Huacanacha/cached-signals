using UnityEngine;
using UnityEngine.UI;
using huacanacha.signal;
using huacanacha.unity.signal;

namespace huacanacha.signals.examples {

public class DataCounterIncrementCommand : ButtonCommand<DataValueSignals, int>
{
    public int incrementAmount = 1;
    public int initialVaue = 42;
    protected override void Command(CachedSignal<int> signal) {
        if (!signal.HasValue) {
            signal.Send(initialVaue);
            return;
        }
        signal.Send(signal.Value+incrementAmount);
    }
    protected override CachedSignal<int> GetSignal(DataValueSignals signalProvider) => signalProvider.counter;
}

}