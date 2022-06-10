// namespace huacanacha.unity.signal
// {
//     using System.Collections.Generic;
//     using UnityEngine;
//     using UnityEngine.SceneManagement;

//     /**
//     * <summary>Scene Signalling manager.</summary>
//     */
//     public sealed class SceneSignalling
//     {
//         private static readonly System.Lazy<SceneSignalling> lazy = new System.Lazy<SceneSignalling>(() => new SceneSignalling());

//         public static SceneSignalling Instance { get { return lazy.Value; } }

//         private SceneSignalling() {
//             SceneManager.sceneLoaded += OnSceneLoaded;
//             SceneManager.sceneUnloaded += OnSceneUnloaded;
//         }

//         private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
//             Debug.Log($"OnSceneLoaded() - ${scene.path}, ${mode}");
//         }
//         private void OnSceneUnloaded(Scene scene) {
//             Debug.Log($"OnSceneUnloaded() - ${scene.path}");
//         }
//     }

//     public class SceneSignalling {

//          Dictionary<Scene, object> _sceneContexts;

//         void Init() {
//             if (_sceneContexts != null) return;

//             // _sceneContexts = new Dictionary<System.Type, object>();
//             // AddAllByType(_signalProviders, false, staticSignalProviders);

//             // var peerSignalProviders = GetComponents<ISignalProvider>();
//             // AddAllByType(_signalProviders, true, peerSignalProviders);
//         }

//         void AddAllByType(Dictionary<System.Type, object> dictionary, bool suppressWarnings, params object[] list) {
//             foreach (object item in list) {
//                 if (dictionary.ContainsKey(item.GetType())) {
//                     if (!suppressWarnings) Debug.LogWarning("Duplicate System or SignalProvider: " + item.GetType());
//                 } else {
//                     dictionary.Add(item.GetType(), item);
//                 }
//             }
//         }

//         private T GetByType<T>(Dictionary<System.Type, object> dictionary) where T : class {
//             object item;
//             if (!dictionary.TryGetValue(typeof(T), out item)) {
//                 return null;
//             }
//             return item as T;
//         }

//         public T GetSignalProvider<T>() where T : class {
//             var sp = GetByType<T>(_signalProviders);
//             if (sp == null) {
//                 Debug.LogWarningFormat("SignalProvider not found: {0}", typeof(T));
//                 return null;
//             }
//             return sp;
//         }

//         static public HierarchySignallingContext FindContext(Transform t) {
//             var signalStation = t.GetComponentInParent<HierarchySignallingContext>();
//             if (signalStation == null) return null;

//             // Ensure initialization regardless of script execution order etc
//             signalStation.Init();

//             return signalStation;
//         }
//         static public HierarchySignallingContext FindContext(MonoBehaviour script) {
//             return FindContext(script.transform);
//         }

//         static public T GetSignalProvider<T>(Transform t) where T : class {
//             var context = FindContext(t);
//             if (context == null) {
//                 Debug.LogWarningFormat("Context not found");
//                 return null;
//             }

//             return context.GetSignalProvider<T>();
//         }
//         static public T GetSignalProvider<T>(MonoBehaviour script) where T : class {
//             return GetSignalProvider<T>(script.transform);
//         }

//     }

// }