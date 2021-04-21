namespace huacanacha.unity.signal
{
    using UnityEngine;

    public static class SignalDiscovery {

        public static T FindSceneSignalProvider<T>(GameObject go) where T : class {
            return null;
        }

        static public HierarchySignallingContext FindContext(Transform t) {
            var signalStation = t.GetComponentInParent<HierarchySignallingContext>();
            if (signalStation == null) return null;

            // Ensure initialization regardless of script execution order etc
            signalStation.Init();

            return signalStation;
        }
        static public HierarchySignallingContext FindContext(MonoBehaviour script) {
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
