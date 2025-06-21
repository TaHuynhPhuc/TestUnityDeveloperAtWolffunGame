using System;
using System.Collections.Generic;
using UnityEngine;

public class GameDataInitializer : MonoBehaviour
{
    public static GameDataInitializer Instance { get; private set; }

    public List<FarmProduction> FarmProductions { get; private set; }
    public LandData LandData { get; private set; }
    public EquipmentData EquipmentData { get; private set; }
    public WorkerData WorkerData { get; private set; }
    public HarvestRulesData HarvestRules { get; private set; }
    public PlayerInitialData PlayerInitial { get; private set; }

    public event Action OnDataLoaded;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeData()
    {
        try
        {
            FarmProductions = CsvDataLoader.LoadFarmProductions();
            LandData = CsvDataLoader.LoadLandData();
            EquipmentData = CsvDataLoader.LoadEquipmentData();
            WorkerData = CsvDataLoader.LoadWorkerData();
            HarvestRules = CsvDataLoader.LoadHarvestRules();
            PlayerInitial = CsvDataLoader.LoadPlayerInitial();

            OnDataLoaded?.Invoke();
            Debug.Log("Game data loaded successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load game data: {ex.Message}");
        }
    }
}
