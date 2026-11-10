using System.Collections.Generic;
using UnityEngine;

public class CrewManager : MonoBehaviour
{
    public static CrewManager instance;

    [Header("Tasks")]
    public List<CrewTask> tasks = new();

    public int TotalCrew { get; private set; }
    public int UnassignedCrew { get; private set; }

    public Dictionary<string, CrewTask> taskLookup;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void Init(int crewCapacity)
    {
        TotalCrew = crewCapacity;
        UnassignedCrew = TotalCrew;

        taskLookup = new Dictionary<string, CrewTask>();
        foreach (var task in tasks)
        {
            task.assigned = 0;
            taskLookup.Add(task.taskId, task);
        }
    }

    public bool AssignCrew(string taskId)
    {
        int crewAssignment = GameManager.instance.airship.crewAssignment;

        if (UnassignedCrew < crewAssignment)
        {
            return false;
        }

        var task = taskLookup[taskId];
        task.assigned += crewAssignment;
        UnassignedCrew -= crewAssignment;
        return true;
    }

    public bool RemoveCrew(string taskId)
    {
        int crewAssignment = GameManager.instance.airship.crewAssignment;

        var task = taskLookup[taskId];
        if (task.assigned < crewAssignment)
        {
            return false;
        }

        task.assigned -= crewAssignment;
        UnassignedCrew += crewAssignment;
        return true;
    }

    public float GetAssigned(string taskId)
    {
        return Mathf.InverseLerp(0, TotalCrew, taskLookup[taskId].assigned);
    }
}
