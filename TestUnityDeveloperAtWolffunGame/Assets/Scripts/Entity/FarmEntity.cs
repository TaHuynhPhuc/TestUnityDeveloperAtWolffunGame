using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FarmEntity : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Button harvestButton;

    private FarmProduction production;
    private float remainingTime;
    private int availableHarvestCount = 0;
    private float postHarvestTimer = -1f;
    private bool isExpired = false;

    private Coroutine growCoroutine;
    public bool HasHarvestable() => availableHarvestCount > 0;
    private bool isAssignedToWorker = false;

    public bool IsAssigned => isAssignedToWorker;

    public void MarkAssigned()
    {
        isAssignedToWorker = true;
    }

    public void HarvestByWorker()
    {
        if (availableHarvestCount <= 0) return;

        RuntimeDataManager.Instance.AddHarvestedItem(production.productionName, availableHarvestCount);
        availableHarvestCount = 0;

        isAssignedToWorker = false;

        harvestButton.gameObject.SetActive(false);
        Debug.Log($"[FarmEntity] Harvested by worker: {production.productionName}");
    }


    private void Start()
    {
        harvestButton.onClick.AddListener(Harvest);
        harvestButton.gameObject.SetActive(false);
    }

    public void Initialize(string productionName)
    {
        production = GameDataInitializer.Instance.FarmProductions
            .Find(p => p.productionName == productionName);

        nameText.text = productionName;
        remainingTime = production.productionTime;

        growCoroutine = StartCoroutine(GrowthRoutine());
    }

    private IEnumerator GrowthRoutine()
    {
        while (!isExpired)
        {
            while (remainingTime > 0f)
            {
                remainingTime -= Time.deltaTime;
                timeText.text = $"{Mathf.CeilToInt(remainingTime)}s";
                yield return null;
            }

            remainingTime += production.productionTime;

            float multiplier = 1f + RuntimeDataManager.Instance.EquipmentLevel * 0.1f;
            int effectiveQuantity = Mathf.RoundToInt(production.productionQuantity * multiplier);
            availableHarvestCount += effectiveQuantity;

            harvestButton.gameObject.SetActive(true);
            UpdateHarvestButtonText();

            if (availableHarvestCount >= production.endOfLifeQuantity)
            {
                isExpired = true;
                postHarvestTimer = GameDataInitializer.Instance.HarvestRules.postHarvestExpirationTime;
                timeText.color = Color.red;
                break;
            }

        }

        while (postHarvestTimer > 0f)
        {
            postHarvestTimer -= Time.deltaTime;
            timeText.text = $"{Mathf.CeilToInt(postHarvestTimer)}s";
            yield return null;
        }

        Destroy(gameObject);
    }

    private void Harvest()
    {
        if (availableHarvestCount <= 0) return;

        RuntimeDataManager.Instance.AddHarvestedItem(production.productionName, availableHarvestCount);

        Debug.Log($"[FarmEntity] Harvested {availableHarvestCount} {production.productionName}.");

        availableHarvestCount = 0;
        harvestButton.gameObject.SetActive(false);
    }

    private void UpdateHarvestButtonText()
    {
        harvestButton.GetComponentInChildren<TextMeshProUGUI>().text = $"x{availableHarvestCount}";
    }

    public string GetEntityName()
    {
        return production != null ? production.productionName : "Unknown";
    }

}
