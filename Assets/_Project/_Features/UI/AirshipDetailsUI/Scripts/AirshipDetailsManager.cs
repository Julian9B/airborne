using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AirshipDetailsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private TextMeshProUGUI historyLabel;
    [SerializeField] private TextMeshProUGUI dimensionsLabel;
    [SerializeField] private TextMeshProUGUI capacityLabel;
    [SerializeField] private TextMeshProUGUI performanceLabel;

    private void OnEnable()
    {
        UpdateDetails();
        UnitSystemManager.Instance.OnSystemChanged += Refresh;
    }

    private void OnDisable()
    {
        UnitSystemManager.Instance.OnSystemChanged -= Refresh;
    }

    void Refresh(UnitSystem system)
    {
        UpdateDetails();
    }

    void UpdateDetails()
    {
        Airship airship = GameManager.instance.airship;

        nameLabel.text = airship.shipName;

        historyLabel.text = airship.history;

        dimensionsLabel.text = 
            $"{Math.Round(UnitConverter.ToDisplay(UnitEnum.Length, airship.length), 1)}{UnitConverter.UnitName(UnitEnum.Length)}\n" +
            $"{Math.Round(UnitConverter.ToDisplay(UnitEnum.Length, airship.diameter), 1)}{UnitConverter.UnitName(UnitEnum.Length)}\n" +
            $"{airship.structure}\n" +
            $"{airship.gas}\n" +
            $"{Math.Round(UnitConverter.ToDisplay(UnitEnum.Volume, airship.volume), 1)}{UnitConverter.UnitName(UnitEnum.Volume)}";

        capacityLabel.text = 
            $"{airship.crewCapacity}\n" +
            $"{airship.passengerCapacity}\n" +
            $"{Math.Round(UnitConverter.ToDisplay(UnitEnum.Mass, airship.cargoCapacity), 1)}{UnitConverter.UnitName(UnitEnum.Mass)}";

        performanceLabel.text = 
            $"{Math.Round(UnitConverter.ToDisplay(UnitEnum.Speed, airship.maxSpeed), 1)}{UnitConverter.UnitName(UnitEnum.Speed)}\n" +
            $"{Math.Round(UnitConverter.ToDisplay(UnitEnum.Length, airship.ceiling), 1)}{UnitConverter.UnitName(UnitEnum.Length)}\n" +
            $"{Math.Round(UnitConverter.ToDisplay(UnitEnum.Distance, airship.range), 1)}{UnitConverter.UnitName(UnitEnum.Distance)}";
    }
}
