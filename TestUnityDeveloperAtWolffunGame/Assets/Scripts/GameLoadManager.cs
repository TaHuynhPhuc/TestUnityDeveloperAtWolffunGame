using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameLoadManager : MonoBehaviour
{
    public static GameLoadManager Instance { get; private set; }

    [SerializeField] private GameLoadState currentState = GameLoadState.None;
    private List<IGameLoadStep> loadSteps = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadScene();
    }

    public void AdvanceTo(GameLoadState state)
    {
        currentState = state;
        Debug.Log($"[GameLoadManager] Entering state: {state}");

        foreach (var step in loadSteps.Where(s => s.TargetState == state))
        {
            step.OnGameStateEntered();
        }
    }

    private void LoadScene()
    {
        StartCoroutine(LoadMainSceneCoroutine());
    }

    private IEnumerator LoadMainSceneCoroutine()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextIndex);
        yield return new WaitUntil(() => asyncLoad.isDone);

        yield return null;

        loadSteps = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IGameLoadStep>()
            .OrderBy(step => (int)step.TargetState)
            .ToList();

        AdvanceTo(GameLoadState.Start);
    }

}
