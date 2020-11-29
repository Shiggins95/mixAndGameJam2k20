using System;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    private bool _isDragging;
    public List<GameObject> ToDisable;
    public float Radius;
    private void Update()
    {
        Vector2 mousePos = MouseHelper.GetMouseCoords(Input.mousePosition);
        
        if (!GameStateManager.CanMoveTurrets)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0) && MouseHelper.ClickedInArea(mousePos, transform.position, Radius))
        {
            _isDragging = true;
            SetDisabled(false);
        }

        if (Input.GetMouseButtonUp(0) && MouseHelper.ClickedInArea(mousePos, transform.position, Radius))
        {
            _isDragging = false;
            SetDisabled(true);
        }

        if (_isDragging)
        {
            transform.position = mousePos;
        }
    }

    private void SetDisabled(bool value)
    {
        foreach (GameObject go in ToDisable)
        {
            go.SetActive(value);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(Radius, Radius));
    }
}