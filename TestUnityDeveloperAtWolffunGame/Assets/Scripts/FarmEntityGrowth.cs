using UnityEngine;
using System;

public class FarmEntityGrowth : MonoBehaviour
{
    private string entityName;
    private float growTime;
    private float timer;
    private bool isGrown;

    public bool IsReadyToHarvest => isGrown;

    public event Action<FarmEntityGrowth> OnGrown;

    public void StartGrowth(string name, float growTime)
    {
        entityName = name;
        this.growTime = growTime;
        timer = 0f;
        isGrown = false;
    }

    private void Update()
    {
        if (isGrown) return;

        timer += Time.deltaTime;
        if (timer >= growTime)
        {
            isGrown = true;
            OnGrown?.Invoke(this);
        }
    }

    public string GetEntityName() => entityName;
}
