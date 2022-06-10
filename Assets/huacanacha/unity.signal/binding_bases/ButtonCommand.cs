using UnityEngine;
using UnityEngine.UI;
using huacanacha.signal;
using huacanacha.unity.signal;

[RequireComponent(typeof(Button))]
public abstract class ButtonCommand<TSignalProvider, TSignalValue> : MonoBehaviour
    where TSignalProvider : class
{
    Button button;
    CachedSignal<TSignalValue> signal;
    void Awake() {
        // Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod().Name}()");
        button = GetComponent<Button>();
        button.onClick.AddListener(ExecuteCommand);
    }

    void ExecuteCommand() {
        if (signal == null || !enabled) return;
        Command(signal);
    }

    protected abstract void Command(CachedSignal<TSignalValue> signal);
    protected abstract CachedSignal<TSignalValue> GetSignal(TSignalProvider signalProvider);

    void OnEnable() {
        // Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod().Name}()");
        if (signal == null) {
            var signalProvider = SignalDiscovery.GetSignalProvider<TSignalProvider>(this);
            if (signalProvider != null) {
                signal = GetSignal(signalProvider);
            }
        }
    }
}
