namespace huacanacha.unity.signal
{
    using System.Collections.Generic;
    using UnityEngine;

    /**
    * <summary>Signalling context for signals within a branch of the Unity hierarchy.</summary>
    */
    public abstract class BaseSignallingContext : MonoBehaviour {

        #region PUBLIC
        public BaseSignallingContext ParentContext {get; protected set;}
        public string Name => gameObject.name;
        public bool IsRoot => ParentContext == null;

        public T GetSignalProvider<T>(bool allowRecursive = true) where T : class {
            // Debug.Log($"{this.GetType().Name}.GetSignalProvider() - {typeof(T)} from: {Name}");
            Initialize();

            // 1 - try the current local context
            var sp = GetByType<T>(_signalProviders);
            if (sp != null) return sp;

            // 2 - try each of the parent contexts
            if (allowRecursive) {
                var current = this.ParentContext;
                while (current != null) {
                    // DON'T recurse as we are walking the tree iteratively
                    sp = current.GetSignalProvider<T>(allowRecursive: false);
                    if (sp != null) return sp;
                    current = current.ParentContext;
                }
                Debug.LogWarning($"SignalProvider '{typeof(T)}' not found from context tree: {TreeToString()}");
            }

            return null;
        }
        #endregion PUBLIC

        // // Disabling "field is never assigned to" warning as it can be set in Unity inspector
        // #pragma warning disable CS0649
        // [SerializeField] protected UnityEngine.Object[] staticSignalProviders;
        // #pragma warning restore CS0649

        Dictionary<System.Type, object> _signalProviders;

        virtual protected void Awake() {
            Initialize();
        }

        internal void Initialize() {
            if (_signalProviders != null) return;
            // Debug.Log($"{this.GetType().Name}.Initialize");

            // AddAllByType(false, staticSignalProviders);

            var peerSignalProviders = GetComponents<ISignalProvider>();
            AddAllByType(true, peerSignalProviders);

            ParentContext = FindParent();
            // Debug.Log($"SignallingContext {Name} parent = {ParentContext?.Name}");

            AfterInitialize();
        }
        
        abstract protected BaseSignallingContext FindParent();
        virtual protected void AfterInitialize() {}

        protected void AddAllByType(bool suppressWarnings, params object[] list) {
            _signalProviders = _signalProviders ?? new Dictionary<System.Type, object>();
            foreach (object item in list) {
                if (_signalProviders.ContainsKey(item.GetType())) {
                    if (!suppressWarnings) Debug.LogWarning("Duplicate SignalProvider: " + item.GetType());
                } else {
                    _signalProviders.Add(item.GetType(), item);
                }
            }
        }

        protected T GetByType<T>(Dictionary<System.Type, object> dictionary) where T : class {
            object item;
            if (!dictionary.TryGetValue(typeof(T), out item)) {
                return null;
            }
            return item as T;
        }

        public string TreeToString() {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(Name);
            var current = this.ParentContext;
            while (current != null) {
                sb.Append($"->{current.Name}");
                current = current.ParentContext;
            }
            return sb.ToString();
        }

    }

}