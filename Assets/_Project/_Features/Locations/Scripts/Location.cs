using UnityEngine;

[CreateAssetMenu(fileName = "NewLocation", menuName = "Location")]
public class Location : ScriptableObject
{
    public string locationName;
    public string shortName;
    public Origin origin;

    public Vector3 startingPoint;
    public Vector3 startingRotation;
}