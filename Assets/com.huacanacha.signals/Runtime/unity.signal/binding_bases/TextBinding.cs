using UnityEngine;
using huacanacha.signal;
using huacanacha.unity.signal;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public abstract class TextBinding<TSignalProvider, TSignalValue, TBoundValue> : MonoBehaviour
    where TSignalProvider : class
{
    private static readonly System.Func<TSignalValue, string> defaultConverter = (v) => v?.ToString() ?? "";
    virtual protected System.Func<TSignalValue, string> Converter {get => defaultConverter;}
    virtual protected string DefaultValue {get => null;}

    SubscriptionReceipt? subscriptionReceipt;

    protected TMPro.TextMeshProUGUI text;
    virtual protected void Awake() {
        // Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod().Name}()");
        text = GetComponent<TMPro.TextMeshProUGUI>();
    }

    // virtual protected TSignalProvider GetSignalProvider(TSignalProvider signalProvider);
    protected abstract CachedSignal<TSignalValue> GetSignal(TSignalProvider signalProvider);

    void OnEnable() {
        // Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod().Name}()");
        subscriptionReceipt?.Unsubscribe(); // Does nothing if no valid receipt

        if (DefaultValue != null) {
            text.text = DefaultValue;
        }
        
        // Note: if efficient enable/disable behaviour is desired, cache the reference to the Signal (and skip subsequent discovery)
        var signalProvider = SignalDiscovery.GetSignalProviderAnywhere<TSignalProvider>(this);
        if (signalProvider != null) {
            subscriptionReceipt = GetSignal(signalProvider).Subscribe(OnValueChanged);
        }
    }
    void OnDisable() {
        // Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod().Name}()");
        subscriptionReceipt?.Unsubscribe();
        subscriptionReceipt = null;
        // ConfigureListener(false);
    }

    void OnValueChanged(TSignalValue value) {
        text.text = Converter(value);
    } 
}
