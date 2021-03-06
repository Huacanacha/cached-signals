using UnityEngine;
using UnityEngine.UI;
using huacanacha.signal;
using huacanacha.unity.signal;

public class ClearDataTextCommand : ButtonCommand<DataSignals, string>
{
    public int incrementAmount = 1;
    protected override void Command(CachedSignal<string> signal) => signal.Send(null);
    protected override CachedSignal<string> GetSignal(DataSignals signalProvider) => signalProvider.text;
}
