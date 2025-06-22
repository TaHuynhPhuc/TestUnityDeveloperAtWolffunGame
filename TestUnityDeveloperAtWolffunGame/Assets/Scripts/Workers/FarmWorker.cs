using TMPro;
using UnityEngine;
using System.Collections;

public class FarmWorker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusText;

    public bool IsBusy { get; private set; } = false;
    private const float taskDuration = 2f;

    public void SetStatus(string text)
    {
        statusText.text = text;
    }

    public void AssignTask(FarmTask task)
    {
        StartCoroutine(PerformTask(task));
    }

    private IEnumerator PerformTask(FarmTask task)
    {
        IsBusy = true;
        SetStatus($"Doing: \n{task.Description}");

        yield return new WaitForSeconds(taskDuration);

        task.Execute?.Invoke();
        SetStatus("Waiting...");
        IsBusy = false;
    }
}
