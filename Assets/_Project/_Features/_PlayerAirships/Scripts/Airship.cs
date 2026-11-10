using UnityEngine;

[CreateAssetMenu(fileName = "NewAirship", menuName = "Airship")]
public class Airship : ScriptableObject
{
    public string shipName;
    public int year;
    public GameObject prefab;
    public Airship previousShip;
    public Origin origin;
    public Vector2 buttonPosition;

    [Tooltip("[m]")]
    public float length;

    [Tooltip("[m]")]
    public float diameter;

    public string structure;

    public string gas;

    [Tooltip("[m3]")]
    public int volume;

    public int crewCapacity;
    public int crewAssignment;

    public int passengerCapacity;

    [Tooltip("[kg]")]
    public int cargoCapacity;

    [Tooltip("[m/s]")]
    public float maxSpeed;

    [Tooltip("[m]")]
    public int ceiling;

    [Tooltip("[m]")]
    public int range;

    [Multiline()]
    public string history;
}