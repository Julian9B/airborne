using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUnitSystem", menuName = "Unit System")]
public class UnitSystem : ScriptableObject
{
    public string systemName;

    public List<Unit> units;
}
