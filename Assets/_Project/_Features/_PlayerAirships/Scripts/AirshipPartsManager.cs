using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipPartsManager : MonoBehaviour
{
    [Header("Propellers")]
    [SerializeField] private List<GameObject> propellers;
    [SerializeField] private float propSpeed = 300.0f;
    [SerializeField] private float minPitch = 0f;
    [SerializeField] private float maxPitch = 2.5f;

    [Header("Lights")]
    [SerializeField] private GameObject navLights;
    [SerializeField] private GameObject mainLight;

    [Header("Mooring")]
    [SerializeField] private LineRenderer cable;
    [SerializeField] private Transform cableStart;
    [SerializeField] private CollisionNotifier cableEnd;
    [SerializeField] private CollisionNotifier mooringProbe;
    [SerializeField] private float maxCableLength = 1f;
    [SerializeField] private float maxCapturedLength = 3f;
    [SerializeField] private float maxDockedLength = 0.5f;
    [SerializeField] private float cableLengthTolerance = 0.01f;
    [SerializeField] private float cableTogglingSpeed = 0.3f;
    [SerializeField] public string mooringPointName = "MooringPoint";

    private PlayerAirshipManager airship;

    private float cableLength = 0f;

    void Awake()
    {
        airship = (PlayerAirshipManager)FindFirstObjectByType(typeof(PlayerAirshipManager));

        cableEnd.OnHit += CableContact;
        mooringProbe.OnHit += ProbeContact;
    }

    void Update()
    {
        UpdatePropellers();
        UpdateLights();
        UpdateCablePosition();
        UpdateCableLength();
        UpdateDistance();
    }

    void OnDestroy()
    {
        cableEnd.OnHit -= CableContact;
        mooringProbe.OnHit -= ProbeContact;
    }

    void UpdatePropellers()
    {
        foreach (var propeller in propellers)
        {
            if (propeller != null)
            {
                AudioSource audioSource = propeller.GetComponent<AudioSource>();
                propeller.transform.Rotate(0, 0, airship.Throttle * propSpeed * Time.deltaTime);
                audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, Math.Abs(airship.Throttle / 3.0f));
            }
        }
    }

    void UpdateLights()
    {
        // nav lights
        if (airship.NavLightsOn == true && navLights.activeSelf == false)
        {
            navLights.SetActive(true);
        }
        else if (airship.NavLightsOn == false && navLights.activeSelf == true)
        {
            navLights.SetActive(false);
        }

        // main light
        if (airship.MainLightOn == true && mainLight.activeSelf == false)
        {
            mainLight.SetActive(true);
        }
        else if (airship.MainLightOn == false && mainLight.activeSelf == true)
        {
            mainLight.SetActive(false);
        }
    }

    void UpdateCablePosition()
    {
        Vector3 newPosition;

        if (airship.Captured == true)
        {
            newPosition = airship.mooringPosition;
        }
        else
        {
            newPosition = new Vector3(cableStart.position.x, cableStart.position.y - cableLength, cableStart.position.z);
        }

        cableEnd.transform.position = newPosition;

        cable.SetPosition(0, cableStart.position);
        cable.SetPosition(1, cableEnd.transform.position);

        airship.cableStartPosition = cableStart.position;
    }

    void UpdateCableLength()
    {
        if (airship.CableOut == true)
        {
            cableLength = Mathf.MoveTowards(cableLength, maxCableLength, cableTogglingSpeed * CrewManager.instance.GetAssigned("mooring") * Time.deltaTime);
        }
        else
        {
            cableLength = Mathf.MoveTowards(cableLength, 0, cableTogglingSpeed * CrewManager.instance.GetAssigned("mooring") * Time.deltaTime);
        }
    }

    void CableContact(Collider other)
    {
        if (cableLength >= maxCableLength - cableLengthTolerance && other.gameObject.name == mooringPointName)
        {
            airship.Capture();
            airship.mooringPosition = other.transform.position;
        }
    }

    void ProbeContact(Collider other)
    {
        if (airship.Captured == true && airship.Docked == false && other.gameObject.name == mooringPointName)
        {
            airship.Dock();
        }
    }

    void UpdateDistance()
    {
        float cableDistance = Vector3.Distance(cableStart.transform.position, cableEnd.transform.position);

        if (airship.Captured == true && cableDistance > maxCapturedLength)
        {
            airship.Uncapture();
        }

        if (airship.Docked == true && cableDistance > maxDockedLength)
        {
            airship.Undock();
        }
    }
}
