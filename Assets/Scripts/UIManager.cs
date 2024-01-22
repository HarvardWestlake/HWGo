using UnityEngine;
using UnityEngine.EventSystems; // Required for event system

public class UIManager : MonoBehaviour
{
    public GameObject panel; // Assign your panel here in the inspector

    // Method to show the panel
    public void ShowPanel()
    {
        panel.SetActive(true);
    }
    
    // Method to hide the panel
    public void HidePanel()
    {
        if(panel.activeSelf)
        {
            panel.SetActive(false);
        }
    }

    // Update method to detect clicks outside the panel
    void Update()
    {
        // Check if the panel is active and if the left mouse button is clicked
        if (panel.activeSelf && Input.GetMouseButtonDown(0))
        {
            // Check if the click is on the panel
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                HidePanel();
            }
        }
    }
}