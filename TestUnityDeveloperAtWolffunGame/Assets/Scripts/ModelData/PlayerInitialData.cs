using System.Collections.Generic;

[System.Serializable]
public class PlayerInitialData
{
    public int startingGold;
    public int startingWorkers;
    public int startingLandSlots;
    public int startingEquipmentLevel;

    public Dictionary<string, int> startingEntities = new Dictionary<string, int>();
}
