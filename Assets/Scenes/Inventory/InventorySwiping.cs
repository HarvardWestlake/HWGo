using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySwiping : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public RectTransform page1;
    public RectTransform page2;
    public float swipeThreshold = 100f;
    private Vector2 startPosition;
    
    private void Start()
    {
        startPosition = page1.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float dragDistance = eventData.delta.x;

        // Move the pages horizontally based on the drag
        page1.anchoredPosition += new Vector2(dragDistance, 0f);
        page2.anchoredPosition += new Vector2(dragDistance, 0f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float distanceMoved = page1.anchoredPosition.x - startPosition.x;

        // Check if the swipe distance is greater than the threshold
        if (Mathf.Abs(distanceMoved) >= swipeThreshold)
        {
            // Determine the direction of the swipe
            if (distanceMoved > 0)
            {
                // Swipe right, go to page 1
                page1.anchoredPosition = startPosition;
                page2.anchoredPosition = new Vector2(startPosition.x + page1.rect.width, 0f);
            }
            else
            {
                // Swipe left, go to page 2
                page1.anchoredPosition = new Vector2(startPosition.x - page1.rect.width, 0f);
                page2.anchoredPosition = startPosition;
            }
        }
        else
        {
            // Not a significant swipe, reset positions
            page1.anchoredPosition = startPosition;
            page2.anchoredPosition = new Vector2(startPosition.x + page1.rect.width, 0f);
        }
    }
}
