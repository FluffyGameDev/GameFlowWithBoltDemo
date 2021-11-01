using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WorldLoadingScheduler : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<bool> m_OnLoadingStateChanged;
    [SerializeField]
    private UnityEvent m_OnGameWorldPostLoad;

    private static List<AsyncOperation> ms_Operations = new List<AsyncOperation>();
    private static bool ms_IsLoading = false;
    private static WorldLoadingScheduler ms_Instance = null;

    private void Awake()
    {
        ms_Instance = this;
    }

    private void Update()
    {
        bool isLoading = ms_Operations.Any(x => !x.isDone);
        if (ms_IsLoading != isLoading)
        {
            ms_IsLoading = isLoading;
            m_OnLoadingStateChanged?.Invoke(ms_IsLoading);

            if (!ms_IsLoading)
            {
                ms_Operations.Clear();
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

        ms_Operations.Add(gameWorldLoadOperation);
        ms_Operations.Add(hudLoadOperation);
    }

    private void UnloadGameWorldInternal()
    {
        ms_Operations.Add(SceneManager.UnloadSceneAsync("GameWorld"));
        ms_Operations.Add(SceneManager.UnloadSceneAsync("HUDScene"));
    }
}