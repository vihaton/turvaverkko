using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPlaneInputScript : MonoBehaviour, ClickableInterface {

    private LookPointMoveScript LPMS;
    private InputHandlerScript inputHandler;
    private bool panning;
    private Vector3 dragOrigin;

    private void Start()
    {
        LPMS = FindObjectOfType<LookPointMoveScript>();
        inputHandler = FindObjectOfType<InputHandlerScript>();
        //StartCoroutine(ChangeDragOrigin());
    }

    public void Clicked()
    {
        panning = true;
        dragOrigin = Input.mousePosition;
    }

    public void Held()
    {
        //Not implemented
    }

	void LateUpdate ()
    {
		if (panning)
        {
            
            Vector3 pos = Camera.main.ScreenToViewportPoint(dragOrigin - Input.mousePosition);
            Vector3 move = new Vector3(pos.x, 0, pos.y);

            LPMS.MoveLookPoint(move);

            if (!Input.GetMouseButton(0))
            {
                panning = false;
                LPMS.PanStopped();
            }
        }
	}

    private IEnumerator ChangeDragOrigin()
    {
        while (true)
        {
            if (panning)
            {
                dragOrigin = Input.mousePosition;
                yield return new WaitForSeconds(0.000001f);
            }

            yield return null;
        }
    }
}
