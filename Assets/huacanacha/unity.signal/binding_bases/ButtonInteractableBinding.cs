using UnityEngine;
using UnityEngine.UI;
using huacanacha.signal;
using huacanacha.unity.signal;

[RequireComponent(typeof(Button))]
public abstract class ButtonInteractableBinding<TSignalProvider> : MonoBehaviour
    where TSignalProvider : class
{
    SubscriptionReceipt? subscriptionReceipt;

    // private static readonly System.Func<TSignalValue, bool> defaultPredicate = (v) => {
    // virtual protected System.Func<TSignalValue, bool> Predicate {get => defaultPredicate;}

    protected Button button;
    virtual protected void Awake() {
        // Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod().Name}()");
        button = GetComponent<Button>();
    }
    protected abstract CachedSignal<bool> GetSignal(TSignalProvider signalProvider);
    // protected abstract bool ButtonInteractable(TSignalValue signalValue);

    void OnEnable() {
        // Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod().Name}()");
        subscriptionReceipt?.Unsubscribe(); // Does nothing if no valid receipt
        
        // Note: if efficient enable/disable behaviour is desired, cache the reference to the Signal (and skip subsequent discovery)
        var signalProvider = SignalDiscovery.GetSignalProvider<TSignalProvider>(this);
        if (signalProvider != null) {
            var signal = GetSignal(signalProvider);
            subscriptionReceipt = signal.Subscribe(OnValueChanged);
            // if (!signal.HasValue) {
            //     OnValueChanged(false);
            // }
        }
    }
    void OnDisable() {
        // Debug.Log($"{System.Reflection.MethodBase.GetCurrentMethod().Name}()");
        subscriptionReceipt?.Unsubscribe();
        subscriptionReceipt = null;
        // ConfigureListener(false);
    }

    void OnValueChanged(bool value) {
        button.interactable = value;
    } 
}
