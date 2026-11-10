using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrewTaskBar : MonoBehaviour
{
    public RawImage background;
    public TMP_Text label;
    public Button plusButton;
    public Button minusButton;

    private CrewTask task;
    private bool isUnassigned;

    public void Init(CrewTask taskData, bool unassigned)
    {
        task = taskData;
        isUnassigned = unassigned;

        background.color = task.backgroundColor;

        plusButton.gameObject.SetActive(!unassigned);
        minusButton.gameObject.SetActive(!unassigned);

        plusButton.onClick.AddListener(OnPlus);
        minusButton.onClick.AddListener(OnMinus);

        Refresh();
    }

    void Refresh()
    {
        if (isUnassigned)
        {
            int value = CrewManager.instance.UnassignedCrew;
            label.text = $"{task.displayName}: {value}";
        }
        else
        {
            label.text = $"{task.displayName}: {task.assigned}";
        }
    }


    void OnPlus()
    {
        if (CrewManager.instance.AssignCrew(task.taskId))
        {
            RefreshAll();
        }
    }

    void OnMinus()
    {
        if (CrewManager.instance.RemoveCrew(task.taskId))
        {
            RefreshAll();
        }
    }

    void RefreshAll()
    {
        foreach (var bar in FindObjectsByType<CrewTaskBar>(FindObjectsSortMode.None))
        {
            bar.Refresh();
        }
    }
}
