using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FarmManager : MonoBehaviour, IGameLoadStep
{
    public static FarmManager Instance { get; private set; }

    [SerializeField] private GameObject landSlotPrefab;
    [SerializeField] private Transform landSlotRoot;
    [SerializeField] private GameObject farmEntityPrefab;

    private readonly List<LandSlot> landSlots = new();

    private EquipmentData equipmentData;

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
            return;
        }
    }

    public void OnGameStateEntered()
    {
        equipmentData = GameDataInitializer.Instance.EquipmentData;
    }

    public void CreateInitialLandSlots(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject landGO = Instantiate(landSlotPrefab, landSlotRoot);
            LandSlot slot = landGO.GetComponent<LandSlot>();
            if (slot != null)
            {
                RegisterLandSlot(slot);
            }
        }

        Debug.Log($"[FarmManager] Spawned {count} land slots.");
    }

    public void RegisterLandSlot(LandSlot slot)
    {
        if (!landSlots.Contains(slot))
        {
            landSlots.Add(slot);
        }
    }

    public void TrySpawnEntity(string entityName)
    {
        if (!RuntimeDataManager.Instance.HasEntity(entityName))
        {
            Debug.LogWarning($"[FarmManager] No more '{entityName}' available to place.");
            return;
        }

        foreach (var slot in landSlots)
        {
            if (!slot.IsOccupied)
            {
                GameObject entityGO = Instantiate(farmEntityPrefab, slot.transform);
                var entity = entityGO.GetComponent<FarmEntity>();
                entity.Initialize(entityName);

                slot.SetOccupied(true);
                RuntimeDataManager.Instance.UseEntity(entityName, 1);

                Debug.Log($"[FarmManager] Spawned '{entityName}' on slot.");
                return;
            }
        }

        Debug.LogWarning("[FarmManager] No available land slots.");
    }

    public int GetOccupiedSlotCount()
    {
        return landSlots.Count(slot => slot.IsOccupied);
    }


    public void AddNewLandSlot()
    {
        GameObject landGO = Instantiate(landSlotPrefab, landSlotRoot);
        LandSlot slot = landGO.GetComponent<LandSlot>();
        if (slot != null)
        {
            RegisterLandSlot(slot);
        }

        Debug.Log("[FarmManager] New land slot added.");
    }

    public int GetTotalSlotCount()
    {
        return landSlots.Count;
    }

    public void UpgradeEquipment()
    {
        equipmentData = GameDataInitializer.Instance.EquipmentData;
        int cost = equipmentData.upgradeCost;

        if (RuntimeDataManager.Instance.SpendGold(cost))
        {
            RuntimeDataManager.Instance.UpgradeEquipment();
            Debug.Log($"[FarmManager] Equipment upgraded to level {RuntimeDataManager.Instance.EquipmentLevel}");
        }
        else
        {
            Debug.LogWarning("[FarmManager] Not enough gold to upgrade equipment.");
        }
    }

}
