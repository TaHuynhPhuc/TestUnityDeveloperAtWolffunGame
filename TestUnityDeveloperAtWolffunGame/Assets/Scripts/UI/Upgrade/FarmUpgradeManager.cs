using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FarmUpgradeManager : MonoBehaviour, IGameLoadStep
{
    [SerializeField] private Button buyLandButton;
    [SerializeField] private Button upgradeEquipmentButton;
    [SerializeField] private Button hireWorkerButton;

    [SerializeField] private TextMeshProUGUI equipmentLevelText;

    public GameLoadState TargetState => GameLoadState.RuntimeInitialized;

    public void OnGameStateEntered()
    {
        buyLandButton.onClick.AddListener(BuyLandSlot);
        upgradeEquipmentButton.onClick.AddListener(UpgradeEquipment);
        hireWorkerButton.onClick.AddListener(HireWorker); 
        UpdateEquipmentLevelText();
    }

    private void BuyLandSlot()
    {
        int landPrice = GameDataInitializer.Instance.LandData.landPricePerSlot;

        if (RuntimeDataManager.Instance.SpendGold(landPrice))
        {
            FarmManager.Instance.AddNewLandSlot();
            Debug.Log($"[FarmUpgradeManager] Bought 1 land slot for {landPrice} gold.");
        }
        else
        {
            Debug.LogWarning("[FarmUpgradeManager] Not enough gold to buy land slot.");
        }
    }

    private void UpgradeEquipment()
    {
        FarmManager.Instance.UpgradeEquipment();
        UpdateEquipmentLevelText();
    }

    private void HireWorker()
    {
        FarmWorkerManager.Instance?.HireWorker();
    }

    private void UpdateEquipmentLevelText()
    {
        equipmentLevelText.text = $"Equipment Level: {RuntimeDataManager.Instance.EquipmentLevel}";
    }
}
