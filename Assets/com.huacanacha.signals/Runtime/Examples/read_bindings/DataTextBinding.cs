using huacanacha.signal;

public class DataTextBinding : TextBinding<DataValueSignals, string, string> {
    protected override CachedSignal<string> GetSignal(DataValueSignals signalProvider) => signalProvider.text;
}
