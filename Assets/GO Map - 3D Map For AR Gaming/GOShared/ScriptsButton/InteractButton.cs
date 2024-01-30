using UnityEngine;
using UnityEngine.EventSystems;

public class PokeballButton : MonoBehaviour, IPointerClickHandler
{
    // Method called when the pokeball button is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        // Call the custom function
        InRange();

        // Prevent the click from being detected by other layers
        eventData.pointerPressRaycast = new RaycastResult();
        eventData.Use();

        //resets volume button
        AudioListener.volume = 1;
    }

    // Custom function to be called when the pokeball is clicked
    private void InRange()
    {
        Debug.Log("InRange function called");
        // Add the functionality you want to be triggered with the button click here
    }
}
