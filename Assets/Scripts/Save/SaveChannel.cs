using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Save/Save Channel")]
public class SaveChannel : ScriptableObject
{
    public delegate void SaveRequestCallback();
    public SaveRequestCallback OnRequestSave;

    public void RaiseRequestSave()
    {
        OnRequestSave?.Invoke();
    }
}