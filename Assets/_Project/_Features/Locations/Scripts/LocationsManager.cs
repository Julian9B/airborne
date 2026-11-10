using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LocationsManager : MonoBehaviour
{
    [SerializeField] private Locations locationDatabase;
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private Button selectButton;
    [SerializeField] private CameraController cameraTarget;

    private int currentIndex;

    void OnEnable()
    {
        LoadLocation(0);
    }

    public void PreviousLocation()
    {
        if(currentIndex > 0)
        {
            LoadLocation(currentIndex - 1);
        }
        else
        {
            LoadLocation(locationDatabase.locations.Length - 1);
        }
    }

    public void NextLocation()
    {
        if(currentIndex < locationDatabase.locations.Length - 1)
        {
            LoadLocation(currentIndex + 1);
        }
        else
        {
            LoadLocation(0);
        }
    }

    private void LoadLocation (int index)
    {
        Location location = locationDatabase.locations[index];
        currentIndex = index;
        
        cameraTarget.SetPosition(location.startingPoint);

        nameLabel.text = $"{location.locationName}, {location.origin.originName}";
        selectButton.onClick.AddListener(() => SelectStartingLocation(location));
    }

    private void SelectStartingLocation(Location location)
    {
        GameManager.instance.startingLocation = location;
    }
}
