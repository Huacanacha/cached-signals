namespace huacanacha.unity.signal
{
    using System.Collections.Generic;
    using UnityEngine;

    /**
    * <summary>Signalling context for signals within a branch of the Unity hierarchy.</summary>
    */
    public class SignallingContext : MonoBehaviour {

        #pragma warning disable CS0649
        [SerializeField] private UnityEngine.Object[] staticSignalProviders;
        #pragma warning restore CS0649

        Dictionary<System.Type, object> _signalProviders;

        SignallingContext _rootContext;
        bool IsRoot { get {return _rootContext == null;} }
        

        void Awake() {
            Init();
        }
        void Start() {
            _rootContext = FindContext(this);
        }

        void Init() {
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

        static public SignallingContext FindContext(Transform t) {
            var signalStation = t.GetComponentInParent<SignallingContext>();
            if (signalStation == null) return null;

            // Ensure initialization regardless of script execution order etc
            signalStation.Init();

            return signalStation;
        }
        static public SignallingContext FindContext(MonoBehaviour script) {
            return FindContext(script.transform);
        }

        static public T GetSignalProvider<T>(Transform t) where T : class {
            var context = FindContext(t);
            if (context == null) {
                Debug.LogWarningFormat("Context not found");
                return null;
            }

            return context.GetSignalProvider<T>();
        }
        static public T GetSignalProvider<T>(MonoBehaviour script) where T : class {
            return GetSignalProvider<T>(script.transform);
        }

    }

}