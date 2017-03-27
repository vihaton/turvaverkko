using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragListenerScript : MonoBehaviour {

    public bool dragging = false;

    private float distance;
    private Coroutine holdTimer;

    void OnMouseDown()
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
    }

    void OnMouseUp()
    {
        dragging = false;
    }

    void Update()
    {
        if (dragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            transform.position = new Vector3(rayPoint.x, transform.position.y, rayPoint.z);
        }
    }

    public void EnableDrag()
    {
        Debug.Log("Drag enabled");
        dragging = true;
    }
}
