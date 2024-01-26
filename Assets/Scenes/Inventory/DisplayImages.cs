using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButtonManager : MonoBehaviour
{
    public List<Button> buttons; // Assign these buttons in the Unity Editor

    public void UpdateButtons(List<int> inventory)
    {
        // First, disable all button images
        foreach (var button in buttons)
        {
            button.gameObject.SetActive(false);
        }

        // Now, enable images for buttons in the inventory
        foreach (var item in inventory)
        {
            int index = item - 1; // Assuming inventory items start at 1
            if (index >= 0 && index < buttons.Count)
            {
                buttons[index].gameObject.SetActive(true);
            }
        }
    }
}
