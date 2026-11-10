using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VersionDisplay : MonoBehaviour
{
    void Start()
    {
        TMP_Text label = GetComponent<TextMeshProUGUI>();
        label.text = $"Version {Application.version}";
    }
}
