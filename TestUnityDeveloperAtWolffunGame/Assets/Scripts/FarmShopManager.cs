using System.Collections.Generic;
using UnityEngine;

public class FarmShopManager : MonoBehaviour, IGameLoadStep
{
    [SerializeField] private Transform shopRoot;
    [SerializeField] private GameObject shopItemPrefab;

    private readonly List<FarmShopItemUI> shopItemUIs = new();

    public GameLoadState TargetState => GameLoadState.RuntimeInitialized;

    public void OnGameStateEntered()
    {
        SpawnShopItems();
    }

    public void RefreshShop()
    {
        SpawnShopItems();
    }

    private void SpawnShopItems()
    {
        foreach (Transform child in shopRoot)
        {
            Destroy(child.gameObject);
        }
        shopItemUIs.Clear();

        List<FarmProduction> shopItems = GameDataInitializer.Instance.FarmProductions;

        foreach (var item in shopItems)
        {
            GameObject go = Instantiate(shopItemPrefab, shopRoot);
            var shopUI = go.GetComponent<FarmShopItemUI>();
            shopUI.Initialize(item);
            shopItemUIs.Add(shopUI);
        }
    }
}
