namespace huacanacha.unity.signal
{
    using System.Collections.Generic;
    using huacanacha.unity.util;
    using UnityEngine;

    /**
    * <summary>Global signalling context that persists across Unity scene changes.</summary>
    */
    public class GlobalSignallingContext : BaseSignallingContext {
        #region SINGLETON
        public static GlobalSignallingContext Instance { get; private set; }

		override protected void Awake() {
			// Debug.Log($"PersistentSingleton<${GetType().Name}> = ${Instance} => ${this}");
			if (Instance != null) {
				Destroy(gameObject);
				return;
			}

			Instance = this;
			this.transform.SetParent(null);
			DontDestroyOnLoad(this.gameObject);

            // Allow the signalling context to initialize now
            base.Awake();
		}

        virtual protected void OnDestroy() {
			if (Instance != this) return;
			Instance = null;
		}
        #endregion SINGLETON

        protected override BaseSignallingContext FindParent() => null;

    }

}