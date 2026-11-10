using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private PlayerAirshipManager airship;
    [SerializeField] private WindZone windZone;

    [SerializeField] private Image outlineFill;
    [SerializeField] private Transform throttlePointer;
    [SerializeField] private Transform windArrow;
    [SerializeField] private Transform compass;
    [SerializeField] private TextMeshProUGUI buoyancyLabel;
    [SerializeField] private TextMeshProUGUI gasLabel;
    [SerializeField] private TextMeshProUGUI ballastLabel;
    [SerializeField] private TextMeshProUGUI airspeedLabel;
    [SerializeField] private TextMeshProUGUI altitudeLabel;
    [SerializeField] private TextMeshProUGUI fuelLabel;

    [SerializeField] private float throttleAngleMultiplier = -36f;

    private int throttleAngle = 0;

    void Update()
    {
        if(GameManager.instance.InFlight == true)
        {
            // health
            outlineFill.fillAmount = airship.Health;

            // wind direction
            ApplyAttitude();

            // target throttle
            ApplyThrottle();

            // buoyancy
            buoyancyLabel.text = $"{Math.Round(airship.Buoyancy, 1)}";

            // gas
            gasLabel.text = $"{Math.Round(airship.Gas * 100, 1)}%";

            // ballast
            ballastLabel.text = $"{Math.Round(airship.Ballast * 100, 1)}%";

            // airspeed
            airspeedLabel.text = $"{Math.Round(UnitConverter.ToDisplay(UnitEnum.Speed, airship.Airspeed / GameManager.instance.worldScale), 1)}{UnitConverter.UnitName(UnitEnum.Speed)}";

            // altitude
            altitudeLabel.text = $"{Math.Round(UnitConverter.ToDisplay(UnitEnum.Length, airship.Altitude / GameManager.instance.worldScale), 1)}{UnitConverter.UnitName(UnitEnum.Length)}";

            // fuel
            fuelLabel.text = $"{Math.Round(airship.Fuel * 100, 1)}%";
        }
    }

    private void ApplyThrottle()
    {
        int targetThrottle = airship.TargetThrottle;

        if (throttleAngle != targetThrottle)
        {
            throttlePointer.localEulerAngles = new Vector3(0, 0, targetThrottle * throttleAngleMultiplier);
            throttleAngle = targetThrottle;

            AudioSource audio = throttlePointer.GetComponent<AudioSource>();
            audio.Play();
        }
    }

    private void ApplyAttitude()
    {
        float airshipAttitude = airship.transform.eulerAngles.y;
        compass.transform.eulerAngles = new Vector3(0, 0, airshipAttitude);

        float windDirection = windZone.transform.eulerAngles.y - airship.transform.eulerAngles.y;
        windArrow.transform.eulerAngles = new Vector3(0, 0, windDirection * -1);
    }
}
