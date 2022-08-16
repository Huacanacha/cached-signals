using UnityEngine;
using huacanacha.signal;
using huacanacha.unity.signal;

[RequireComponent(typeof(TMPro.TMP_InputField))]
public abstract class InputFieldCommand<TSignalProvider, TSignalValue> : MonoBehaviour
    where TSignalProvider : class
{
    TMPro.TMP_InputField inputField;
    CachedSignal<TSignalValue> signal;
    void Awake() {
        // Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod().Name}()");
        inputField = GetComponent<TMPro.TMP_InputField>();
        inputField.onEndEdit.AddListener(ExecuteCommand);
    }

    void ExecuteCommand(string value) {
        if (signal == null || !enabled) return;
        Command(signal, value);
    }

    protected abstract void Command(CachedSignal<TSignalValue> signal, string value);
    protected abstract CachedSignal<TSignalValue> GetSignal(TSignalProvider signalProvider);

    void OnEnable() {
        // Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod().Name}()");
        if (signal == null) {
            var signalProvider = SignalDiscovery.GetSignalProviderAnywhere<TSignalProvider>(this);
            if (signalProvider != null) {
                signal = GetSignal(signalProvider);
            }
        }
    }
}
