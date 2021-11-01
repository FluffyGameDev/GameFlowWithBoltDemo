using UnityEngine;

public class HUDBehaviour : MonoBehaviour
{
    [SerializeField]
    private UIChannel m_UIChannel;

    private void Awake()
    {
        m_UIChannel.OnRequestHUDVisible += gameObject.SetActive;
    }

    private void OnDestroy()
    {
        m_UIChannel.OnRequestHUDVisible -= gameObject.SetActive;
    }
}
