using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WorldLoadingScheduler : MonoBehaviour
{
    [SerializeField]
    private FlowChannel m_FlowChannel;
    [SerializeField]
    private FlowTrigger m_LoadingEndFlowTrigger;
    [SerializeField]
    private UnityEvent<bool> m_OnLoadingStateChanged;
    [SerializeField]
    private UnityEvent m_OnGameWorldPostLoad;

    private List<AsyncOperation> m_Operations = new List<AsyncOperation>();
    private bool m_IsLoading = false;

    private static WorldLoadingScheduler ms_Instance = null;

    private void Awake()
    {
        ms_Instance = this;
    }

    private void Update()
    {
        bool isLoading = m_Operations.Any(x => !x.isDone);
        if (m_IsLoading != isLoading)
        {
            m_IsLoading = isLoading;
            m_OnLoadingStateChanged?.Invoke(m_IsLoading);

            if (!m_IsLoading)
            {
                m_FlowChannel.RaiseFlowTrigger(m_LoadingEndFlowTrigger);
                m_Operations.Clear();
            }
        }
    }

    public static void LoadGameWorld()
    {
        ms_Instance.LoadGameWorldInternal();
    }

    public static void UnloadGameWorld()
    {
        ms_Instance.UnloadGameWorldInternal();
    }

    private void LoadGameWorldInternal()
    {
        //This is bad, don't use the scene names. Use the IDs or any other scene identification method.
        AsyncOperation gameWorldLoadOperation = SceneManager.LoadSceneAsync("GameWorld", LoadSceneMode.Additive);
        AsyncOperation hudLoadOperation = SceneManager.LoadSceneAsync("HUDScene", LoadSceneMode.Additive);

        gameWorldLoadOperation.completed += (x => m_OnGameWorldPostLoad.Invoke());

        m_Operations.Add(gameWorldLoadOperation);
        m_Operations.Add(hudLoadOperation);
    }

    private void UnloadGameWorldInternal()
    {
        m_Operations.Add(SceneManager.UnloadSceneAsync("GameWorld"));
        m_Operations.Add(SceneManager.UnloadSceneAsync("HUDScene"));
    }
}