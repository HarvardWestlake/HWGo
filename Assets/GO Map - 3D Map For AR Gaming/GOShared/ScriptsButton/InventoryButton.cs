using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryButton : MonoBehaviour
{
    public void LoadInventory()
    {
        SceneManager.LoadScene("InventoryAll");
    }
}
