namespace huacanacha.unity.signal
{
    using UnityEngine;

    /**
    * <summary>Signalling context for signals within a branch of the Unity hierarchy.</summary>
    */
    public class SceneSignallingContext : BaseSignallingContext {

        protected override void AfterInitialize() {
            SceneSignallingContextManager.Instance.SetSceneContext(gameObject.scene, this);
        }

        protected override BaseSignallingContext FindParent() {
            return SignalDiscovery.FindGlobalContext();
        }
    }

}