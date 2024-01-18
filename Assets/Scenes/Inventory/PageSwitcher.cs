using UnityEngine;
using UnityEngine.UI;

public class PageSwitcher : MonoBehaviour
{
    public GameObject currPage; // Reference to the first UI page
    public GameObject otherPage; // Reference to the second UI page

    private Button button;

    private void Start()
    {
        // Get the Button component attached to this GameObject
        button = GetComponent<Button>();

        // Add an event listener for the button click
        button.onClick.AddListener(SwitchPage);
    }

    private void SwitchPage()
    {
        otherPage.SetActive(true);
        currPage.SetActive(false);
    }
}