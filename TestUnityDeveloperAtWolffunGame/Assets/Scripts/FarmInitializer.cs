using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmInitializer : MonoBehaviour, IGameLoadStep
{
    [SerializeField] private GameObject landSlotPrefab;
    [SerializeField] private Transform landSlotRoot;

    public GameLoadState TargetState => GameLoadState.RuntimeInitialized;

    public void OnGameStateEntered()
    {
        int slotCount = GameDataInitializer.Instance.PlayerInitial.startingLandSlots;
        FarmManager.Instance.CreateInitialLandSlots(slotCount);

        GameLoadManager.Instance.AdvanceTo(GameLoadState.SceneSpawned);
    }
}