using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandlerScript : MonoBehaviour {

    public float holdingTime;
    public float coolDown;
    public bool currentlyHandlingInput = false;
    public bool incomingInput = false;
    public GameObject[] popups;

    private GameObject lastItemPointed = null;
    private Vector3 pointHit;
    private bool notOnCooldown = true;
    private float t = 0;
    private float timer = 0;

    private void Start()
    {
        StartCoroutine(HoldTimer());
    }

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && notOnCooldown && NoPopupsActive())
        {
            t = Time.time;
            currentlyHandlingInput = true;
            GetObjectPointed();
        }
        
        if (incomingInput)
        {
            HandleInputEvent();
        }

    }

    private bool NoPopupsActive()
    {
        foreach (GameObject go in popups)
        {
            if (go.activeSelf)
                return false;
        }
        return true;
    }

    private void HandleInputEvent()
    {
        incomingInput = false;
        float delta = (Time.time - t);

        if (delta > holdingTime)
        {
            //Debug.Log("Last item held, " + lastItem);

            GetObjectPointed();
            TryToInteract(true);
        }
        else if (delta < holdingTime)
        {
            //Debug.Log("Last item clicked, " + lastItem);
            if (lastItemPointed != null)
                TryToInteract(false);
        }
        t = 0;

        StartCoroutine(CooldownTimer());
    }

    private void TryToInteract(bool isHold)
    {
        ClickableInterface clickable = lastItemPointed.GetComponent<ClickableInterface>();
        if (clickable == null)
            return;
        if (isHold)
        {
            clickable.Held();
        } else
        {
            clickable.Clicked();
        }
    }

    private void GetObjectPointed()
    {
        lastItemPointed = null;
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f) && hit.transform.gameObject)
        {
            pointHit = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            lastItemPointed = hit.transform.gameObject;
            Debug.Log("Last item hit: " + lastItemPointed.name);
        }
    }

    private IEnumerator HoldTimer()
    {
        while (true)
        {
            if (currentlyHandlingInput)
            {
                if (lastItemPointed.tag.Equals("WorldPlane") || timer > holdingTime || Input.GetMouseButtonUp(0))
                {
                    incomingInput = true;
                    currentlyHandlingInput = false;
                    timer = 0;
                } else
                {
                    timer += Time.deltaTime;
                }
            }
            yield return null;
        }
    }

    private IEnumerator CooldownTimer()
    {
        notOnCooldown = false;
        float deltaTime = 0;

        while (deltaTime < coolDown)
        {
            deltaTime += Time.deltaTime;
            yield return null;
        }

        notOnCooldown = true;
    }

    public Vector3 GetPointHit()
    {
        return pointHit;
    }
}
