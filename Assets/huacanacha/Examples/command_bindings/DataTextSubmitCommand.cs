using UnityEngine;
using UnityEngine.UI;
using huacanacha.signal;
using huacanacha.unity.signal;

public class DataTextSubmitCommand : InputFieldCommand<DataSignals, string>
{
    protected override void Command(CachedSignal<string> signal, string value) => signal.Send(value);
    protected override CachedSignal<string> GetSignal(DataSignals signalProvider) => signalProvider.text;
}