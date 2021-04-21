namespace huacanacha.unity.signal
{
    using System.Collections.Generic;
    using UnityEngine;

    /**
    * <summary>Signalling context for signals within a branch of the Unity hierarchy.</summary>
    */
    public class HierarchySignallingContext : MonoBehaviour {

        // Disabling "field is never assigned to" warning as it can be set in Unity inspector
        #pragma warning disable CS0649
        [SerializeField] private UnityEngine.Object[] staticSignalProviders;
        #pragma warning restore CS0649

        Dictionary<System.Type, object> _signalProviders;

        HierarchySignallingContext _parentContext;
        bool IsRoot { get {return _parentContext == null;} }
        

        void Awake() {
            Init();
        }
        void Start() {
            _parentContext = SignalDiscovery.FindContext(this);
        }

        internal void Init() {
            if (_signalProviders != null) return;

            _signalProviders = new Dictionary<System.Type, object>();
            AddAllByType(_signalProviders, false, staticSignalProviders);

            var peerSignalProviders = GetComponents<ISignalProvider>();
            AddAllByType(_signalProviders, true, peerSignalProviders);
        }

        void AddAllByType(Dictionary<System.Type, object> dictionary, bool suppressWarnings, params object[] list) {
            foreach (object item in list) {
                if (dictionary.ContainsKey(item.GetType())) {
                    if (!suppressWarnings) Debug.LogWarning("Duplicate System or SignalProvider: " + item.GetType());
                } else {
                    dictionary.Add(item.GetType(), item);
                }
            }
        }

        private T GetByType<T>(Dictionary<System.Type, object> dictionary) where T : class {
            object item;
            if (!dictionary.TryGetValue(typeof(T), out item)) {
                return null;
            }
            return item as T;
        }

        public T GetSignalProvider<T>() where T : class {
            var sp = GetByType<T>(_signalProviders);
            if (sp == null) {
                Debug.LogWarningFormat("SignalProvider not found: {0}", typeof(T));
                return null;
            }
            return sp;
        }

    }

}