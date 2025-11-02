using UnityEngine;

public class MouseTracker : MonoBehaviour
{
    public RectTransform mouseIndicator;
    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        mouseIndicator.position = mousePos;
    }
}
