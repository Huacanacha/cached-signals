namespace huacanacha.unity.signal
{
    using System;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using System.Collections.Generic;

    public sealed class SceneSignallingContextManager {
        #region SINGLETON
        private static readonly Lazy<SceneSignallingContextManager> lazy = new Lazy<SceneSignallingContextManager>(() => new SceneSignallingContextManager());
        public static SceneSignallingContextManager Instance { get { return lazy.Value; } }
        private SceneSignallingContextManager() => Initialize();
        #endregion

        Dictionary<Scene, SceneSignallingContext> contexts = new Dictionary<Scene, SceneSignallingContext>();

        void Initialize() {
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnSceneUnloaded(Scene current) {
            contexts.Remove(current);
        }

        public SceneSignallingContext GetSceneContext(Scene scene) {
            // Debug.Log($"GetSceneContext() - {scene.name}");
            if (contexts.TryGetValue(scene, out var context)) {
                return context;
            }

            // First time asked we check the scene root objects for a SceneSignallingContext,
            // and if not found set the lookup value to null
            foreach (var go in scene.GetRootGameObjects()) {
                var newContext = go.GetComponent<SceneSignallingContext>();
                if (newContext != null) {
                    contexts[scene] = newContext;
                    return newContext;
                }
            }
            contexts[scene] = null;

            return null;
        }

        public bool SetSceneContext(Scene scene, SceneSignallingContext context) {
            // Debug.Log($"SetSceneContext() - {scene.name}, {context.Name}");
            if (contexts.TryGetValue(scene, out var currenSceneContext)) {
                if (context != currenSceneContext) {
                    Debug.LogWarning($"Can't replace scene signalling context: scene={scene.name}, new context={context.name}, old context={contexts[scene].Name}");
                }
                return false;
            }
            contexts[scene] = context;
            return true;
        }
    }

}
