using UnityEngine;

[System.Serializable]
public class CrewTask
{
    public string taskId;
    public string displayName;
    public Color backgroundColor;

    [HideInInspector]
    public int assigned;
}
