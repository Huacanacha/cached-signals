using huacanacha.signal;

namespace huacanacha.signals.examples {

public class LoadingProgressBinding : TextBinding<GameStateSignals, float, string> {

    protected override CachedSignal<float> GetSignal(GameStateSignals signalProvider) => signalProvider.sceneLoadingProgress;

    override protected System.Func<float, string> Converter {get => ValueToString;}

    static string ValueToString(float progress) {
        var percent = progress*100;
        return $"<b>Loading progress {percent:0}%</b>\n\n<size=70%>{(progress >= 1 ? "Click or press space to continue..." : " ")}";
    }
}

}