using UnityEngine;
using huacanacha.signal;

namespace huacanacha.signals.examples {

public class DateBinding : TextBinding<DateSignals, string, string> {
    protected override CachedSignal<string> GetSignal(DateSignals signalProvider) => signalProvider.date;
}

}