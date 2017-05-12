using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragListenerScript : MonoBehaviour {

    public bool dragging = false;

    private float distance;
    private Coroutine holdTimer;
    private PawnDataStruct pawnData;

    private void Start()
    {
        pawnData = this.GetComponent<PawnDataStruct>();
    }

    void OnMouseDown()
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
    }

    void OnMouseUp()
    {
        pawnData.UpdateAll();
        dragging = false;
    }

    void Update()
    {
        if (dragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            transform.position = new Vector3(rayPoint.x, 1.5f, rayPoint.z);
        }
    }

    public void EnableDrag()
    {
        Debug.Log("Drag enabled");
        dragging = true;
    }
}
