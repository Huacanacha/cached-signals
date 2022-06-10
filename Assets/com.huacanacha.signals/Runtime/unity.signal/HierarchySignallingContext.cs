namespace huacanacha.unity.signal
{
    using UnityEngine;

    /**
    * <summary>Signalling context for signals within a branch of the Unity hierarchy.</summary>
    */
    public class HierarchySignallingContext : BaseSignallingContext {

        protected override BaseSignallingContext FindParent() {
            return SignalDiscovery.FindParentContext(this.transform);
        }
    }

}