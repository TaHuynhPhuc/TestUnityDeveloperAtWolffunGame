using TMPro;
using UnityEngine;

public class FarmEntityButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;

    private string entityName;

    public void Setup(string name, int amount)
    {
        entityName = name;
        UpdateText(amount);
    }

    public void OnClick()
    {
        if (RuntimeDataManager.Instance.HasEntity(entityName))
        {
            FarmManager.Instance.TrySpawnEntity(entityName);
            int newAmount = RuntimeDataManager.Instance.GetEntityAmount(entityName);
            UpdateText(newAmount);
        }
    }

    private void UpdateText(int amount)
    {
        buttonText.text = $"{entityName}\n x{amount}";
    }
}
