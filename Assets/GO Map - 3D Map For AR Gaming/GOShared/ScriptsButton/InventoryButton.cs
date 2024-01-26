using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryButton : MonoBehaviour
{
    public void LoadInventory()
    {
        SceneManager.LoadScene("InventoryAll");

        //resets volume button
        AudioListener.volume = 1;
    }
}
