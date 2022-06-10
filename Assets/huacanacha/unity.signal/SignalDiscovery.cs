namespace huacanacha.unity.signal
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public static class SignalDiscovery {

        static public BaseSignallingContext FindContext(Transform t, bool allowRecursive = true) {
            // Debug.Log($"FindContext() - ${t?.gameObject.name}, ${allowRecursive}");

            var context = t?.GetComponent<HierarchySignallingContext>();
            if (context != null) return context;

            return FindParentContext(t);
        }

        static public BaseSignallingContext FindParentContext(Transform t, bool allowRecursive = true) {
            // Debug.Log($"FindParentContext() - ${t?.gameObject.name}, ${allowRecursive}");
            var parentTransform = t.parent;

            // 1 - look for local Hierarchy context
            var context = parentTransform?.GetComponentInParent<HierarchySignallingContext>();
            if (context != null) return context;

            if (allowRecursive) {
                if (t != null) {
                    return FindSceneContext(t.gameObject.scene, allowRecursive);
                }
                return FindGlobalContext();
            }

            return null;
        }

        static public BaseSignallingContext FindSceneContext(Scene scene, bool allowRecursive = true) {
            // Debug.Log($"FindSceneContext() - ${scene.name}, ${allowRecursive}");
            BaseSignallingContext context = null;
            
            // 1 - look for Scene context
            context = SceneSignallingContextManager.Instance.GetSceneContext(scene);
            if (context != null) return context;
            if (!allowRecursive) return null;
            
            // 2 - look for global context
            return FindGlobalContext();
        }

        static public BaseSignallingContext FindGlobalContext() {
            // Debug.Log($"FindGlobalContext()");
            var context = GlobalSignallingContext.Instance;
            if (context != null) return context;

            if (context == null) {
                // Debug.Log("context == null");
                foreach (var go in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) {
                    var globalContext = go.transform.GetComponent<GlobalSignallingContext>();
                    if (globalContext != null) {
                        globalContext.Initialize();
                        context = globalContext;
                        break;
                    }
                }
                if (context != null) return context;
            }
            // Debug.Log($"signalling context=${context?.GetType()}");

            return null;
        }

        static public T GetSignalProvider<T>(MonoBehaviour script) where T : class {
            return GetSignalProvider<T>(script.transform);
        }

        static public T GetSignalProvider<T>(Transform t, bool allowRecursive = true) where T : class {
            var context = FindContext(t, allowRecursive: allowRecursive);
            if (context == null) {
                Debug.LogWarningFormat("Context not found");
                return null;
            }

            return context.GetSignalProvider<T>(allowRecursive);
        }

        static public T GetSceneSignalProvider<T>(MonoBehaviour script) where T : class {
            return GetSceneSignalProvider<T>(script.gameObject.scene);
        }
        static public T GetSceneSignalProvider<T>(Scene scene, bool allowRecursive = true) where T : class {
            var context = FindSceneContext(scene, allowRecursive: true);
            if (context == null) return null;

            return context.GetSignalProvider<T>(false);
        }

        static public T GetGlobalSignalProvider<T>() where T : class {
            var context = FindGlobalContext();
            if (context == null) {
                Debug.LogWarningFormat("Context not found");
                return null;
            }

            return context.GetSignalProvider<T>(false);
        }

    }
}
