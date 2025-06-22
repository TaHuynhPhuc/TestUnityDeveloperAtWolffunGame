using System.Collections.Generic;
using UnityEngine;

public class FarmWorkerManager : MonoBehaviour, IGameLoadStep
{
    public static FarmWorkerManager Instance { get; private set; }

    [SerializeField] private GameObject workerPrefab;
    [SerializeField] private Transform workerRoot;

    private readonly List<FarmWorker> workers = new();

    public GameLoadState TargetState => GameLoadState.RuntimeInitialized;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void OnGameStateEntered()
    {
        int initialWorkerCount = GameDataInitializer.Instance.PlayerInitial.startingWorkers;
        for (int i = 0; i < initialWorkerCount; i++)
        {
            SpawnWorker();
        }
    }

    private void Update()
    {
        foreach (var worker in workers)
        {
            if (!worker.IsBusy)
            {
                var task = FarmManager.Instance.GetNextAvailableTask();
                if (task != null)
                {
                    worker.AssignTask(task);
                }
                else
                {
                    worker.SetStatus("Idle...");
                }
            }
        }
    }

    public void HireWorker()
    {
        const int hireCost = 500;

        if (RuntimeDataManager.Instance.SpendGold(hireCost))
        {
            GameObject go = Instantiate(workerPrefab, workerRoot);
            var worker = go.GetComponent<FarmWorker>();

            workers.Add(worker);
            worker.SetStatus("Idle...");
            Debug.Log("[FarmWorkerManager] Worker hired and added.");
        }
        else
        {
            Debug.LogWarning("[FarmWorkerManager] Not enough gold to hire worker.");
        }
    }

    private void SpawnWorker()
    {
        GameObject go = Instantiate(workerPrefab, workerRoot);
        var worker = go.GetComponent<FarmWorker>();
        workers.Add(worker);
        worker.SetStatus("Idle...");
    }
}
