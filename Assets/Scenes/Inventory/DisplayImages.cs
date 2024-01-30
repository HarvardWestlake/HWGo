using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButtonManager : MonoBehaviour
{
    public List<Image> buttonImages; // Assign the Images in the Unity Editor

    public void UpdateButtonImages(List<int> inventory)
    {
        // First, disable all images
        foreach (var image in buttonImages)
        {
            image.enabled = false;
        }

        // Now, enable images for buttons in the inventory
        foreach (var item in inventory)
        {
            int index = item - 1; // Assuming inventory items start at 1
            if (index >= 0 && index < buttonImages.Count)
            {
                buttonImages[index].enabled = true;
            }
        }
    }
}
