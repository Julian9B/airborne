using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string header;
    [Multiline]
    [SerializeField] private string content;

    private Coroutine showRoutine;

    public void OnPointerEnter(PointerEventData eventData)
    {
        showRoutine = StartCoroutine(ShowDelayed());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (showRoutine != null)
        {
            StopCoroutine(showRoutine);
            showRoutine = null;
        }

        TooltipSystem.Hide();
    }

    private IEnumerator ShowDelayed()
    {
        yield return new WaitForSeconds(0.5f);
        TooltipSystem.Show(header, content);
    }
}
