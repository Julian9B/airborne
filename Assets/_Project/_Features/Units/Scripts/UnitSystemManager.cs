using System;
using UnityEngine;

public class UnitSystemManager : MonoBehaviour
{
    public static UnitSystemManager Instance { get; private set; }

    [SerializeField] private UnitSystems database;

    public UnitSystem CurrentSystem { get; private set; }

    public event Action<UnitSystem> OnSystemChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetSystem(int index)
    {
        if (index < 0 || index >= database.systems.Count)
        {
            Debug.LogError($"Invalid unit system index: {index}");
            return;
        }

        ApplySystem(database.systems[index]);
    }

    public void SetSystem(string systemName)
    {
        var system = database.systems.Find(s => s.systemName == systemName);

        if (system == null)
        {
            Debug.LogError($"Unit system '{systemName}' not found");
            return;
        }

        ApplySystem(system);
    }

    private void ApplySystem(UnitSystem system)
    {
        if (CurrentSystem == system)
        {
            return;
        }

        CurrentSystem = system;
        UnitConverter.CurrentSystem = system;

        OnSystemChanged?.Invoke(system);
    }
}
