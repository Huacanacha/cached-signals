using UnityEngine;
using UnityEngine.UI;
using huacanacha.signal;
using huacanacha.unity.signal;

namespace huacanacha.signals.examples {

public class DataTextSubmitCommand : InputFieldCommand<DataValueSignals, string>
{
    protected override void Command(CachedSignal<string> signal, string value) => signal.Send(value);
    protected override CachedSignal<string> GetSignal(DataValueSignals signalProvider) => signalProvider.text;
}

}