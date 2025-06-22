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
        else
        {
            Debug.LogWarning($"[FarmShopItemUI] Not enough gold to buy {batchAmount} {data.productionName}.");
        }
    }

    private void Sell()
    {
        int currentAmount = RuntimeDataManager.Instance.GetEntityAmount(data.productionName);
        if (currentAmount < batchAmount)
        {
            Debug.LogWarning($"[FarmShopItemUI] Not enough {data.productionName} to sell.");
            return;
        }

        RuntimeDataManager.Instance.UseEntity(data.productionName, batchAmount);
        RuntimeDataManager.Instance.RemoveHarvestedItem(data.productionName, batchAmount);
        int totalGold = data.productPrice * batchAmount;
        RuntimeDataManager.Instance.AddGold(totalGold);

        FarmEntityButtonSpawner.Instance?.RefreshButtons();
    }

}
