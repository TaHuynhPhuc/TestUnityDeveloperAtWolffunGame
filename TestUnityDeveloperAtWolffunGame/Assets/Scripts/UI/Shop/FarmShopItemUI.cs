using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FarmShopItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI buyPriceText;
    [SerializeField] private TextMeshProUGUI sellPriceText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button sellButton;

    private FarmProduction data;
    private const int batchAmount = 10;

    public void Initialize(FarmProduction production)
    {
        data = production;

        infoText.text = $"{data.productionName}\nBuy: {data.productPurchasePrice} \n Sell: {data.productPrice}";

        buyPriceText.text = $"x{batchAmount}";
        sellPriceText.text = $"x{batchAmount}";

        buyButton.onClick.AddListener(Buy);
        sellButton.onClick.AddListener(Sell);
    }

    private void Buy()
    {
        int totalCost = data.productPurchasePrice * batchAmount;
        if (RuntimeDataManager.Instance.SpendGold(totalCost))
        {
            RuntimeDataManager.Instance.AddEntity(data.productionName, batchAmount);
            FarmEntityButtonSpawner.Instance?.RefreshButtons();
        }
    }

    private void Sell()
    {
        int currentHarvest = RuntimeDataManager.Instance.GetHarvestedAmount(data.productionName);
        if (currentHarvest < batchAmount)
        {
            Debug.LogWarning($"[FarmShopItemUI] Not enough harvested {data.productionName} to sell.");
            return;
        }

        if (RuntimeDataManager.Instance.SellHarvestedItem(data.productionName, batchAmount))
        {
            int totalGold = data.productPrice * batchAmount;
            RuntimeDataManager.Instance.AddGold(totalGold);
        }
    }

}
