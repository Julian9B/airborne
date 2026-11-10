using UnityEngine;
using TMPro;

public class CrewTasksManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI allCrewLabel;
    [SerializeField] private CrewTaskBar barPrefab;
    [SerializeField] private Transform container;
    [SerializeField] private Color unassignedColor;

    void OnEnable()
    {
        BuildUI();
    }

    void BuildUI()
    {
        allCrewLabel.text = $"All crew memebers: {CrewManager.instance.TotalCrew}";

        foreach (Transform bar in container)
        {
            Destroy(bar.gameObject);
        }

        var unassignedTask = new CrewTask
        {
            taskId = "unassigned",
            displayName = "Unassigned",
            backgroundColor = unassignedColor,
            assigned = CrewManager.instance.UnassignedCrew,
        };

        var unassignedBar = Instantiate(barPrefab, container);
        unassignedBar.Init(unassignedTask, true);

        foreach (var task in CrewManager.instance.tasks)
        {
            var bar = Instantiate(barPrefab, container);
            bar.Init(task, false);
        }
    }
}
