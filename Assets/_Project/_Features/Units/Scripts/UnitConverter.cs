using UnityEngine;

public static class UnitConverter
{
    public static UnitSystem CurrentSystem;

    public static float ToDisplay(UnitEnum unitEnum, float baseValue)
    {
        var unit = CurrentSystem.units.Find(u => u.unitEnum == unitEnum);

        if (unit == null)
        {
            Debug.LogError($"Missing unit for {unitEnum}");
            return baseValue;
        }

        return (baseValue) / unit.multiplierToBase;
    }

    public static string UnitName(UnitEnum unitEnum)
    {
        var unit = CurrentSystem.units.Find(u => u.unitEnum == unitEnum);

        return unit?.displayName ?? "?";
    }
}
