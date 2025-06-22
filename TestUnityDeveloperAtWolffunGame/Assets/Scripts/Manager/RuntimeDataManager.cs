using System.Collections.Generic;
using UnityEngine;

public class RuntimeDataManager : MonoBehaviour, IGameLoadStep
{
    public static RuntimeDataManager Instance { get; private set; }

    public int CurrentGold { get; private set; }
    public int EquipmentLevel { get; private set; }
    public int CurrentWorkers { get; private set; }

    public Dictionary<string, int> CurrentEntityAmounts { get; private set; } = new();
    public Dictionary<string, int> HarvestedItems { get; private set; } = new();

    public GameLoadState TargetState => GameLoadState.DataLoaded;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void OnGameStateEntered()
    {
        Initialize(GameDataInitializer.Instance.PlayerInitial);
        GameLoadManager.Instance.AdvanceTo(GameLoadState.RuntimeInitialized);
    }

    private void Initialize(PlayerInitialData initial)
    {
        CurrentGold = 10000000; // or initial.startingGold;
        CurrentWorkers = initial.startingWorkers;
        EquipmentLevel = initial.startingEquipmentLevel;

        foreach (var kv in initial.startingEntities)
            CurrentEntityAmounts[kv.Key.Trim()] = kv.Value;

        Debug.Log("[RuntimeDataManager] Initialized.");
    }

    public bool HasEntity(string name) => GetEntityAmount(name) > 0;
    public int GetEntityAmount(string name) => CurrentEntityAmounts.TryGetValue(name.Trim(), out var count) ? count : 0;

    public void AddEntity(string name, int amount)
    {
        name = name.Trim();
        if (!CurrentEntityAmounts.ContainsKey(name)) CurrentEntityAmounts[name] = 0;
        CurrentEntityAmounts[name] += amount;
    }

    public void UseEntity(string name, int amount = 1)
    {
        name = name.Trim();
        if (GetEntityAmount(name) >= amount)
            CurrentEntityAmounts[name] -= amount;
        else
            Debug.LogWarning($"[RuntimeDataManager] Not enough {name} to use.");
    }

    public bool HasAnyEntity()
    {
        foreach (var kv in CurrentEntityAmounts)
        {
            if (kv.Value > 0) return true;
        }
        return false;
    }

    public void AddHarvestedItem(string name, int amount)
    {
        name = name.Trim();
        if (!HarvestedItems.ContainsKey(name)) HarvestedItems[name] = 0;
        HarvestedItems[name] += amount;
        Debug.Log($"[RuntimeDataManager] Harvested {amount} {name}, total: {HarvestedItems[name]}");
    }

    public int GetHarvestedAmount(string name)
    {
        name = name.Trim();
        return HarvestedItems.TryGetValue(name, out var count) ? count : 0;
    }

    public bool SellHarvestedItem(string name, int amount)
    {
        name = name.Trim();
        if (GetHarvestedAmount(name) >= amount)
        {
            HarvestedItems[name] -= amount;
            return true;
        }
        return false;
    }

    public void AddGold(int amount)
    {
        CurrentGold += amount;
    }

    public bool SpendGold(int amount)
    {
        if (CurrentGold >= amount)
        {
            CurrentGold -= amount;
            return true;
        }
        return false;
    }

    public void UpgradeEquipment() => EquipmentLevel++;
}
