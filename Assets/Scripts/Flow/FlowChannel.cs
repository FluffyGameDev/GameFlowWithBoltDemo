using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Flow/Flow Channel")]
public class FlowChannel : ScriptableObject
{
    public delegate void FlowTriggerCallback(FlowTrigger trigger);
    public FlowTriggerCallback OnFlowTrigger;

    public void RaiseFlowTrigger(FlowTrigger trigger)
    {
        OnFlowTrigger?.Invoke(trigger);
    }
}