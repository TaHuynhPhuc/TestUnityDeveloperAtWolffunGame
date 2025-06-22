using TMPro;
using UnityEngine;
using System.Text;

public class FarmHUDManager : MonoBehaviour, IGameLoadStep
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI landText;
    [SerializeField] private TextMeshProUGUI harvestedListText;

    private bool checkRuntimeInitialized = false;

    public GameLoadState TargetState => GameLoadState.RuntimeInitialized;

    public void OnGameStateEntered()
    {
        checkRuntimeInitialized = true;
    }

    private void Update()
    {
        if (checkRuntimeInitialized)
        {
            UpdateGoldUI();
            UpdateLandUI();
            UpdateHarvestedUI();
        }
    }

    private void UpdateGoldUI()
    {
        goldText.text = $"Gold: {RuntimeDataManager.Instance.CurrentGold}";
    }

    private void UpdateLandUI()
    {
        int totalSlots = FarmManager.Instance.GetTotalSlotCount();
        int usedSlots = FarmManager.Instance.GetOccupiedSlotCount();
        int freeSlots = totalSlots - usedSlots;

        landText.text = $"Land: {usedSlots}/{totalSlots} used | {freeSlots} free";
    }

    private void UpdateHarvestedUI()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var kv in RuntimeDataManager.Instance.HarvestedItems)
        {
            sb.AppendLine($"{kv.Key}: x{kv.Value}");
        }

        harvestedListText.text = sb.ToString();
    }
}
