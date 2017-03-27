using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandlerScript : MonoBehaviour {

    public float holdingTime;
    public bool handlingInput = false;
    public bool incomingInput = false;

    private GameObject lastItem;
    private float t = 0;
    private float timer = 0;

    private void Start()
    {
        StartCoroutine(HoldTimer());
    }

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            t = Time.time;
            handlingInput = true;
            GetObjectPointed();
        }
        
        if (incomingInput)
        {
            incomingInput = false;
            float delta = (Time.time - t);

            if (delta > holdingTime)
            {
                Debug.Log("Last item held, " + lastItem);

                GetObjectPointed();
                lastItem.GetComponent<DragListenerScript>().EnableDrag();
            }
            else if (delta < holdingTime)
            {
                Debug.Log("Last item clicked, " + lastItem);
                if (lastItem != null)
                    FindObjectOfType<PawnHandlerScript>().ShowPawnInformation(lastItem);
            }
            t = 0;
        }
        
    }

    private void GetObjectPointed()
    {
        lastItem = null;
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f) && hit.transform.gameObject.tag.Contains("SafetyNetEntry"))
        {
            lastItem = hit.transform.gameObject;
        }
    }

    private IEnumerator HoldTimer()
    {
        while (true)
        {
            if (handlingInput)
            {
                if (timer > holdingTime || Input.GetMouseButtonUp(0))
                {
                    incomingInput = true;
                    handlingInput = false;
                    timer = 0;
                } else
                {
                    timer += Time.deltaTime;
                }
            }
            yield return null;
        }
    }
}
