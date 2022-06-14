
using huacanacha.signal;

namespace huacanacha.signals.examples {

class ActivateInitialSceneBinding : ButtonAction<GameStateSignals> {
    protected override ActionSignal GetSignal(GameStateSignals signalProvider) => signalProvider.activateInitialScene;
}

}