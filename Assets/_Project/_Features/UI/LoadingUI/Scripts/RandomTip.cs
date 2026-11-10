using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class RandomTip : MonoBehaviour
{
    public Tips database;

    void Start()
    {
        var text = GetComponent<TextMeshProUGUI>();

        int randomIndex  = Random.Range(0, database.tips.Length);

        text.text = database.tips[randomIndex].content;
    }
}
