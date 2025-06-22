using UnityEngine;

public class LandSlot : MonoBehaviour
{
    public bool IsOccupied { get; private set; }

    private void Start()
    {
        FarmManager.Instance.RegisterLandSlot(this);
    }

    public void SetOccupied(bool value)
    {
        IsOccupied = value;
    }
}
