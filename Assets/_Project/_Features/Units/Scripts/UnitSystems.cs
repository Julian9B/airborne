using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitSystems", menuName = "Unit Systems Database")]
public class UnitSystems : ScriptableObject
{
    public List<UnitSystem> systems;

    public UnitSystem GetSystem(string name)
    {
        return systems.Find(s => s.systemName == name);
    }
}
