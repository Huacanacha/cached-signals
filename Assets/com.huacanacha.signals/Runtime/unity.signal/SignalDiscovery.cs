namespace huacanacha.unity.signal
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public static class SignalDiscovery {

        static public T GetSignalProvider<T>() where T : class {
            return GetSceneSignalProvider<T>();
        }
        static public T GetSignalProvider<T>(MonoBehaviour script) where T : class {
            return GetSceneSignalProvider<T>(script.gameObject.scene);
        }

        static public T GetSignalProviderAnywhere<T>() where T : class {
            return GetSceneSignalProvider<T>() ?? GetAppSignalProvider<T>();
        }
        static public T GetSignalProviderAnywhere<T>(MonoBehaviour script) where T : class {
            return GetLocalSignalProvider<T>(script.transform, searchSceneAndApp: true);
        }

        /// <summary>
        /// Finds the requested signal provider <typeparamref name="T" /> in the HierarchySignallingContext in the parents of the specified scripts transform.
        /// </summary>
        /// <returns>The signal provider, or NULL if context or provider are not found.</returns>
        static public T GetLocalSignalProvider<T>(MonoBehaviour script) where T : class {
            return GetLocalSignalProvider<T>(script.transform, false);
        }

        /// <summary>
        /// Finds the requested signal provider <typeparamref name="T" /> in the HierarchySignallingContext in the parents of the specified transform.
        /// <param name="searchSceneAndApp">Also searches the SceneSignallingContext and GobalSignallingContext, if not found locally.
        /// </summary>
        /// <returns>The signal provider, or NULL if context or provider are not found.</returns>
        static public T GetLocalSignalProvider<T>(Transform t, bool searchSceneAndApp = false) where T : class {
            var context = FindContext(t, allowRecursive: searchSceneAndApp);
            if (context == null) {
                Debug.LogWarningFormat("Context not found");
                return null;
            }

            GetSceneSignalProvider<MonoBehaviour>(t.gameObject.scene);

            return context.GetSignalProvider<T>(searchSceneAndApp);
        }

        /// <summary>
        /// Finds the requested signal provider <typeparamref name="T" /> in the SceneSignallingContext for the currently active scene.
        /// </summary>
        /// <returns>The signal provider, or NULL if context or provider are not found.</returns>
        static public T GetSceneSignalProvider<T>() where T : class {
            return GetSceneSignalProvider<T>(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }

        /// <summary>
        /// Finds the requested signal provider <typeparamref name="T" /> in the SceneSignallingContext for the scene of the specified script.
        /// </summary>
        /// <returns>The signal provider, or NULL if context or provider are not found.</returns>
        static public T GetSceneSignalProvider<T>(MonoBehaviour script) where T : class {
            return GetSceneSignalProvider<T>(script.gameObject.scene);
        }

        /// <summary>
        /// Finds the requested signal provider <typeparamref name="T" /> in the SceneSignallingContext for the specified scene.
        /// If <paramref name="allowRecursive" /> is true, also searches the global signalling context if needed.
        /// </summary>
        /// <returns>The signal provider, or NULL if context or provider are not found.</returns>
        static public T GetSceneSignalProvider<T>(Scene scene, bool allowRecursive = false) where T : class {
            var context = FindSceneContext(scene, allowRecursive: allowRecursive);
            if (context == null) return null;

            return context.GetSignalProvider<T>(false);
        }

        /// <summary>
        /// Finds the requested signal provider <typeparamref name="T" /> in the GlobalSignallingContext.
        /// </summary>
        /// <returns>The signal provider, or NULL if context or provider are not found.</returns>
        static public T GetAppSignalProvider<T>() where T : class {
            var context = FindGlobalContext();
            if (context == null) {
                Debug.LogWarningFormat("Context not found");
                return null;
            }

            return context.GetSignalProvider<T>(false);
        }

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

        static public BaseSignallingContext FindSceneContext(Scene scene, bool allowRecursive = false) {
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

    }
}
