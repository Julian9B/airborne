using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class RandomCrashMessage : MonoBehaviour
{
    public CrashMessages database;

    void OnEnable()
    {
        var text = GetComponent<TextMeshProUGUI>();

        int randomIndex = Random.Range(0, database.crashMessages.Length);

        text.text = database.crashMessages[randomIndex].content;
    }
}
