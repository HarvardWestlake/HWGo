using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySwiping : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public RectTransform page1;
    public RectTransform page2;
    public float swipeThreshold = 100f;

    private Vector2 startPosition;
    private Vector2 targetPositionPage1;
    private Vector2 targetPositionPage2;

    private bool isPage1Active = true;
    private bool isDragging = false;

    private void Start()
    {
        startPosition = page1.anchoredPosition;
        targetPositionPage1 = startPosition;
        targetPositionPage2 = new Vector2(startPosition.x - page1.rect.width, 0f);

        // Initial log to indicate the starting page
        Debug.Log("Starting Page: " + (isPage1Active ? "Page 1" : "Page 2"));
    }

    public void OnDrag(PointerEventData eventData)
    {
        float dragDistance = eventData.delta.x;

        // Check if we are dragging
        if (isDragging)
        {
            // Move the pages horizontally within bounds
            Vector2 newPage1Position = page1.anchoredPosition + new Vector2(dragDistance, 0f);
            Vector2 newPage2Position = page2.anchoredPosition + new Vector2(dragDistance, 0f);

            // Ensure only one page is visible at a time
            newPage1Position.x = Mathf.Clamp(newPage1Position.x, startPosition.x - page1.rect.width, startPosition.x);
            newPage2Position.x = Mathf.Clamp(newPage2Position.x, startPosition.x, startPosition.x + page1.rect.width);

            page1.anchoredPosition = newPage1Position;
            page2.anchoredPosition = newPage2Position;

            // Log the current position of the view
            Debug.Log("Current Position: " + page1.anchoredPosition.x);
        }

        // Determine if we should start dragging
        if (Mathf.Abs(dragDistance) >= swipeThreshold && !isDragging)
        {
            isDragging = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float distanceMoved = page1.anchoredPosition.x - startPosition.x;

        // Check if we were dragging and the swipe distance is significant
        if (isDragging && Mathf.Abs(distanceMoved) >= swipeThreshold)
        {
            // Determine the direction of the swipe
            if (distanceMoved > 0 && !isPage1Active)
            {
                // Swipe right, go to page 1
                page1.anchoredPosition = targetPositionPage1;
                page2.anchoredPosition = targetPositionPage2;
                isPage1Active = true;
            }
            else if (distanceMoved < 0 && isPage1Active)
            {
                // Swipe left, go to page 2
                page1.anchoredPosition = targetPositionPage2;
                page2.anchoredPosition = targetPositionPage1;
                isPage1Active = false;
            }

            // Log the page that is currently selected after the swipe
            Debug.Log("Current Page: " + (isPage1Active ? "Page 1" : "Page 2"));
        }

        // Reset dragging state
        isDragging = false;
    }
}
