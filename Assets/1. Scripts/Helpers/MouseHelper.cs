using UnityEngine;

public static class MouseHelper
{
    public static Vector2 GetMouseCoords(Vector2 position)
    {
        return Camera.main.ScreenToWorldPoint(position);
    }

    public static bool ClickedInArea(Vector2 mousePos, Vector2 area, float checkDistance)
    {
        return Mathf.Abs(mousePos.x - area.x) <= checkDistance &&
               Mathf.Abs(mousePos.y - area.y) <= checkDistance;
    }
}