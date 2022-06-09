
using huacanacha.signal;

class ActivateInitialSceneBinding : ButtonAction<GameStateSignals> {
    protected override ActionSignal GetSignal(GameStateSignals signalProvider) => signalProvider.activateInitialScene;
}