using System.Collections.Generic;
using UnityEngine;

public class RuntimeDataManager : MonoBehaviour, IGameLoadStep
{
    public static RuntimeDataManager Instance { get; private set; }

    public int CurrentGold { get; private set; }
    public int CurrentWorkers { get; private set; }
    public int EquipmentLevel { get; private set; }

    public Dictionary<string, int> CurrentEntityAmounts { get; private set; } = new();
    public Dictionary<string, int> HarvestedItems { get; private set; } = new();

    public GameLoadState TargetState => GameLoadState.DataLoaded;

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
        }
    }

    public void OnGameStateEntered()
    {
        Initialize(GameDataInitializer.Instance.PlayerInitial);
        GameLoadManager.Instance.AdvanceTo(GameLoadState.RuntimeInitialized);
    }

    private void Initialize(PlayerInitialData initial)
    {
        //CurrentGold = initial.startingGold;
        CurrentGold = 10000000;
        CurrentWorkers = initial.startingWorkers;
        EquipmentLevel = initial.startingEquipmentLevel;

        CurrentEntityAmounts = new Dictionary<string, int>();

        foreach (var kv in initial.startingEntities)
        {
            string key = kv.Key.Trim();
            CurrentEntityAmounts[key] = kv.Value;
        }

        Debug.Log("[RuntimeDataManager] Initialized runtime data.");
    }

    public void AddHarvestedItem(string name, int amount)
    {
        name = name.Trim();
        if (!HarvestedItems.ContainsKey(name))
            HarvestedItems[name] = 0;

        HarvestedItems[name] += amount;
        Debug.Log($"[RuntimeDataManager] Stored {amount} {name}, total: {HarvestedItems[name]}");
    }

    public bool HasEntity(string name)
    {
        return CurrentEntityAmounts.TryGetValue(name.Trim(), out int count) && count > 0;
    }


    public void AddEntity(string name, int amount)
    {
        name = name.Trim();
        if (!CurrentEntityAmounts.ContainsKey(name))
            CurrentEntityAmounts[name] = 0;

        CurrentEntityAmounts[name] += amount;
        Debug.Log($"[RuntimeDataManager] Added {amount} {name}, total: {CurrentEntityAmounts[name]}");
    }

    public void AddGold(int amount)
    {
        CurrentGold += amount;
        Debug.Log($"[RuntimeDataManager] Gold increased by {amount}, current: {CurrentGold}");
    }

    public bool SpendGold(int amount)
    {
        if (CurrentGold >= amount)
        {
            CurrentGold -= amount;
            Debug.Log($"[RuntimeDataManager] Spent {amount} gold, remaining: {CurrentGold}");
            return true;
        }

        Debug.LogWarning($"[RuntimeDataManager] Not enough gold to spend {amount}");
        return false;
    }

    public int GetEntityAmount(string name)
    {
        return CurrentEntityAmounts.TryGetValue(name.Trim(), out int count) ? count : 0;
    }

    public void UpgradeEquipment()
    {
        EquipmentLevel++;
        Debug.Log($"[RuntimeDataManager] Equipment upgraded to level {EquipmentLevel}");
    }

    public void UseEntity(string name, int amount)
    {
        name = name.Trim();
        if (HasEntity(name) && CurrentEntityAmounts[name] >= amount)
        {
            CurrentEntityAmounts[name] -= amount;
            Debug.Log($"[RuntimeDataManager] Used {amount} {name}, remaining: {CurrentEntityAmounts[name]}");
        }
        else
        {
            Debug.LogWarning($"[RuntimeDataManager] Cannot use {amount} {name}, not enough quantity.");
        }
    }

    public void RemoveHarvestedItem(string name, int amount)
    {
        name = name.Trim();
        if (HarvestedItems.TryGetValue(name, out int current) && current >= amount)
        {
            HarvestedItems[name] -= amount;
            if (HarvestedItems[name] <= 0)
            {
                HarvestedItems.Remove(name);
            }

            Debug.Log($"[RuntimeDataManager] Removed {amount} {name}, remaining: {HarvestedItems.GetValueOrDefault(name, 0)}");
        }
        else
        {
            Debug.LogWarning($"[RuntimeDataManager] Not enough {name} to remove.");
        }
    }

}
