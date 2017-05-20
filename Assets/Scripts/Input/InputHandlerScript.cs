using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandlerScript : MonoBehaviour {

    public float holdingTime;
    public float coolDown;
    public bool currentlyHandlingInput = false;
    public bool incomingInput = false;
    public GameObject screenPopups;
    public List<GameObject> popups;

    private ClickableInterface lastItemPointed = null;
    private Vector3 pointHit;
    public bool notOnCooldown = true;
    private float t = 0;
    private float timer = 0;

    private void Start()
    {
        popups = new List<GameObject>();
        GetScenePopups();
        StartCoroutine(HoldTimer());
    }

    private void GetScenePopups()
    {
        for (int i = 0; i < screenPopups.transform.childCount; i++)
        {
            Transform child = screenPopups.transform.GetChild(i);
            if (child.parent.name.Equals(screenPopups.name))
            {
                popups.Add(child.gameObject);
            }
        }
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
            TryToInteract(false);
        }

        t = 0;

        StartCoroutine(CooldownTimer());
    }

    private void TryToInteract(bool isHold)
    {
        if (lastItemPointed == null)
            return;
        if (isHold)
        {
            lastItemPointed.Held();
        } else
        {
            lastItemPointed.Clicked();
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
            ClickableInterface clickHandler = hit.transform.gameObject.GetComponent<ClickableInterface>();
            if (clickHandler != null)
                lastItemPointed = clickHandler;

            if (Debug.isDebugBuild)
                Debug.Log("Last item hit: " + hit.transform.gameObject.name);
        }
    }

    private IEnumerator HoldTimer()
    {
        while (true)
        {
            if (currentlyHandlingInput)
            {
                if (lastItemPointed.isOnlyHeld() || Input.GetMouseButtonUp(0) || timer > holdingTime)
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

    public void ButtonPressed()
    {
        StartCoroutine(CooldownTimer());
    }

    public Vector3 GetPointHit()
    {
        return pointHit;
    }
}
