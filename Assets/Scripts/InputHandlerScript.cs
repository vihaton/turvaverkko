using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandlerScript : MonoBehaviour {
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pressed on mouse");
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                GameObject objectHit = hit.transform.gameObject;
                Debug.Log("Object: " + objectHit.ToString());

                if (objectHit.tag.Contains("SafetyNetEntry"))
                {
                    objectHit.GetComponent<DragListenerScript>().EnableDrag();
                }
            }
        }
    }
}
