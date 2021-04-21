using UnityEngine;
using huacanacha.signal;
using huacanacha.unity.signal;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public abstract class TextBinding<TSignalProvider, TSignalValue, TBoundValue> : MonoBehaviour
    where TSignalProvider : class
{
    private static readonly System.Func<TSignalValue, string> defaultConverter = (v) => v?.ToString() ?? "";
    public System.Func<TSignalValue, string> converter = defaultConverter;

    TMPro.TextMeshProUGUI text;
    // SystemsSignals _signals;
    // SystemThatDoesSomething _system;
    CachedSignal<TSignalValue> _signal;
    void Awake() {
        // Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod().Name}()");
        text = GetComponent<TMPro.TextMeshProUGUI>();
    }

    void ConfigureListener(bool add) {
        if (_signal == null) return;
        if (add) {
            _signal.Subscribe(OnValueChanged);
        } else {
            _signal.UnsubscribeByCallback(OnValueChanged);
        }
    }

    protected abstract CachedSignal<TSignalValue> GetSignal(TSignalProvider signalProvider);

    void OnEnable() {
        // Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod().Name}()");
        if (_signal == null) {
            var signalProvider = SignalDiscovery.GetSignalProvider<TSignalProvider>(this);
            if (signalProvider != null) {
                _signal = GetSignal(signalProvider);
            }
        }
        ConfigureListener(true);
    }
    void OnDisable() {
        // Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod().Name}()");
        ConfigureListener(false);
    }

    void OnValueChanged(TSignalValue value) {
        text.text = converter(value);
    } 
}
