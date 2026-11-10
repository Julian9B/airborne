using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private CameraController cameraTarget;

    [SerializeField] private Vector3 displayPosition;

    void OnEnable()
    {
        cameraTarget.SetPosition(displayPosition);
        GameManager.instance.LoadAirshipPosition();
        GameManager.instance.GetAirship();
    }
}
