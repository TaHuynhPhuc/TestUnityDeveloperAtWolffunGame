using System.Collections.Generic;
using UnityEngine;

public class FarmEntityButtonSpawner : MonoBehaviour, IGameLoadStep
{
    public static FarmEntityButtonSpawner Instance { get; private set; }

    [SerializeField] private GameObject farmEntityButtonPrefab;
    [SerializeField] private Transform buttonRoot;

    private readonly List<FarmEntityButtonUI> buttonUIs = new();

    public GameLoadState TargetState => GameLoadState.RuntimeInitialized;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnGameStateEntered()
    {
        SpawnButtons();
        GameLoadManager.Instance.AdvanceTo(GameLoadState.SceneSpawned);
    }

    public void RefreshButtons()
    {
        SpawnButtons();
    }

    private void SpawnButtons()
    {
        foreach (Transform child in buttonRoot)
        {
            Destroy(child.gameObject);
        }
        buttonUIs.Clear();

        List<FarmProduction> entities = GameDataInitializer.Instance.FarmProductions;

        foreach (var entity in entities)
        {
            string name = entity.productionName;
            int amount = RuntimeDataManager.Instance.GetEntityAmount(name);

            GameObject buttonGO = Instantiate(farmEntityButtonPrefab, buttonRoot);
            var buttonUI = buttonGO.GetComponent<FarmEntityButtonUI>();
            buttonUI.Setup(name, amount);

            buttonUIs.Add(buttonUI);
        }
    }
}
