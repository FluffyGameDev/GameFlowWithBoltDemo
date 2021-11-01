using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/UI/UI Channel")]
public class UIChannel : ScriptableObject
{
    public delegate void HUDDisplayCallback(bool isVisible);
    public HUDDisplayCallback OnRequestHUDVisible;

    public void RaiseRequestHUDVisible(bool isVisible)
    {
        OnRequestHUDVisible?.Invoke(isVisible);
    }
}