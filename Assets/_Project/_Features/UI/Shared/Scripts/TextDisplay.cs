using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TextDisplay : MonoBehaviour
{
    [Header("Source File")]
    [SerializeField] private TextAsset sourceFile;

    private TMP_Text textComponent;

    private void OnEnable()
    {
        textComponent = GetComponent<TMP_Text>();
        DisplayCredits();
    }

    private void DisplayCredits()
    {
        if (textComponent != null && sourceFile != null)
            textComponent.text = sourceFile.text;
    }
}
