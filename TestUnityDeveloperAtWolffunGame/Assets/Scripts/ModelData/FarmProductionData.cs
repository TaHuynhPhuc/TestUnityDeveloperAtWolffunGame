using System.Collections.Generic;

[System.Serializable]
public class FarmProduction
{
    public string productionName;
    public float productionTime;
    public int productionQuantity;
    public int endOfLifeQuantity;
    public int productPrice;
    public int productPurchasePrice;
}

public class FarmProductionData
{
    public List<FarmProduction> productions = new List<FarmProduction>();

    public void AddFarmProductionData(string name, float time, int quantity, int endOfLife, int price, int purchasePrice)
    {
        productions.Add(new FarmProduction
        {
            productionName = name,
            productionTime = time,
            productionQuantity = quantity,
            endOfLifeQuantity = endOfLife,
            productPrice = price,
            productPurchasePrice = purchasePrice
        });
    }
}
