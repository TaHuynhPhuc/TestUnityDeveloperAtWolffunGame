using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CsvDataLoader
{
    public static List<FarmProduction> LoadFarmProductions()
    {
        var productions = new List<FarmProduction>();
        var lines = LoadLines(CsvPaths.FarmProductions);

        for (int i = 1; i < lines.Length; i++)
        {
            var cells = lines[i].Trim().Split(',');
            if (cells.Length < 6) continue;

            productions.Add(new FarmProduction
            {
                productionName = cells[0],
                productionTime = float.Parse(cells[1]),
                productionQuantity = int.Parse(cells[2]),
                endOfLifeQuantity = int.Parse(cells[3]),
                productPrice = int.Parse(cells[4]),
                productPurchasePrice = int.Parse(cells[5])
            });
        }

        return productions;
    }

    public static LandData LoadLandData()
    {
        var cells = LoadLines(CsvPaths.LandData)[1].Trim().Split(',');
        return new LandData
        {
            maxLandSlots = int.Parse(cells[0]),
            landPricePerSlot = int.Parse(cells[1])
        };
    }

    public static EquipmentData LoadEquipmentData()
    {
        var cells = LoadLines(CsvPaths.EquipmentData)[1].Trim().Split(',');
        return new EquipmentData
        {
            upgradeCost = int.Parse(cells[0]),
            productivityIncrease = float.Parse(cells[1])
        };
    }

    public static WorkerData LoadWorkerData()
    {
        var cells = LoadLines(CsvPaths.WorkerData)[1].Trim().Split(',');
        return new WorkerData
        {
            hireCost = int.Parse(cells[0]),
            workTimePerTask = float.Parse(cells[1])
        };
    }

    public static HarvestRulesData LoadHarvestRules()
    {
        var cells = LoadLines(CsvPaths.HarvestRules)[1].Trim().Split(',');
        return new HarvestRulesData
        {
            postHarvestExpirationTime = float.Parse(cells[0])
        };
    }

    public static PlayerInitialData LoadPlayerInitial()
    {
        var lines = LoadLines(CsvPaths.PlayerInitial);
        var headers = lines[0].Trim().Split(',');
        var values = lines[1].Trim().Split(',');

        var data = new PlayerInitialData
        {
            startingGold = int.Parse(values[0]),
            startingWorkers = int.Parse(values[1]),
            startingLandSlots = int.Parse(values[2]),
            startingEquipmentLevel = int.Parse(values[3]),
            startingEntities = new Dictionary<string, int>()
        };

        for (int i = 4; i < headers.Length; i++)
        {
            string name = headers[i].Trim();
            int amount = int.Parse(values[i].Trim());
            data.startingEntities[name] = amount;
        }

        return data;
    }

    private static string[] LoadLines(string relativePath)
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, relativePath);
        if (!File.Exists(fullPath))
            throw new Exception($"Missing file at path: {fullPath}");

        return File.ReadAllLines(fullPath);
    }
}
