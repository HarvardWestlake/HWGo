using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public GameObject panel;
    public Button facultyImageButton;
    public Button closeButton;

    void Start()
    {
        // Add event listeners to the buttons
        facultyImageButton.onClick.AddListener(ShowPanel);
        closeButton.onClick.AddListener(HidePanel);
    }

    public void ShowPanel()
    {
        panel.SetActive(true);
    }

    public void HidePanel()
    {
        if (panel.activeSelf)
        {
            panel.SetActive(false);
        }
    }

    void Update()
    {
        // Optional: Hide panel when tapping outside
        if (panel.activeSelf && Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && !panel.GetComponentInChildren<Collider2D>().OverlapPoint(touch.position))
            {
                HidePanel();
            }
        }
    }
}