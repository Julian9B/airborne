using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    public static TooltipSystem instance;

    [SerializeField] private Tooltip tooltip;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public static void Show(string header, string content)
    {
        instance.tooltip.gameObject.SetActive(true);
        instance.tooltip.SetText(header, content);
        instance.tooltip.SetPosition();
    }

    public static void Hide()
    {
        instance.tooltip.gameObject.SetActive(false);
    }
}
