using UnityEngine;

public class UIManager : MonoBehaviour
{
    [HideInInspector] public GameObject activePopup;

    public void ToggleVisibility(GameObject targetUI)
    {
        if (targetUI != null)
        {
            targetUI.SetActive(!targetUI.activeSelf);
            TogglePopup(activePopup);
        }
    }

    public void TogglePopup(GameObject targetPopup)
    {
        if (targetPopup != null)
        {
            if(targetPopup == activePopup)
            {
                targetPopup.SetActive(false);
                activePopup = null;
                return;
            }

            if(activePopup != null)
            {
                activePopup.SetActive(false);
            }
            
            targetPopup.SetActive(true);
            activePopup = targetPopup;
        }
        
    }

    public void Quit() {
        Application.Quit();
    }
}