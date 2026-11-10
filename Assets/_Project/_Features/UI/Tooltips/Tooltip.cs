using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using TMPro;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private TextMeshProUGUI content;
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private int characterLimit;
    [SerializeField] private InputSystemUIInputModule input;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string headerText, string contentText)
    {
        header.text = headerText;
        content.text = contentText;

        int headerLength = header.text.Length;
        int contentLength = content.text.Length;
        layoutElement.enabled = (headerLength > characterLimit || contentLength > characterLimit);
    }

    public void SetPosition()
    {
        Vector2 position = Mouse.current.position.ReadValue();
        transform.position = position;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;
        rectTransform.pivot = new Vector2(pivotX, pivotY);
    }
}
