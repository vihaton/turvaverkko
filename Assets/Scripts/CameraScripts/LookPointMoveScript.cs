using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPointMoveScript : MonoBehaviour {

    public float lerpTime;
    public float baseSpeed;
    public GameObject safetyNetArea;

    private SafetyNetAdminScript SNAS;
    private GameObject previousLookPoint;
    private GameObject previousGoal;
    private Coroutine moveAndRotate;

    private void Start()
    {
        SNAS = FindObjectOfType<SafetyNetAdminScript>();
        GameObject lookPoint;

        if (safetyNetArea.transform.childCount > 0)
        {
            lookPoint = safetyNetArea.transform.GetComponentsInChildren<Transform>()[1].gameObject;
        } else
        {
            lookPoint = safetyNetArea;
        }

        previousLookPoint = lookPoint;
    }

    public void MoveLookPoint(Vector3 movementVector)
    {
        gameObject.transform.Translate(movementVector * baseSpeed, Space.World);
    }

    public void MoveBackToPreviousPoint()
    {
        MoveTo(previousLookPoint);
    }

    public void MoveTo(GameObject goal, float overriddenLerpTime)
    {
        previousLookPoint = previousGoal;
        previousGoal = goal;

        Vector3 startPos = transform.position;
        Vector3 endPos = goal.transform.position;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = goal.transform.rotation;

        Debug.Log("Moving to: (" + endPos.x + ", " + endPos.y + ", " + endPos.z + ")");

        moveAndRotate = StartCoroutine(MoveAndRotate(startPos, endPos, startRot, endRot, goal));
    }

    public void MoveTo(GameObject goal)
    {
        MoveTo(goal, lerpTime);
    }

    internal void PanStopped()
    {
        GameObject closestSafetyNet = SNAS.GetClosestSafetyNet(transform.position);
        SNAS.ChangeSafetyNet(closestSafetyNet);
        MoveTo(closestSafetyNet, 0.00001f);
    }

    IEnumerator MoveAndRotate(Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot, GameObject goal, float overriddenLerpTime)
    {
        float deltaTime = 0;
        while (deltaTime < overriddenLerpTime)
        {
            float percentage = deltaTime / overriddenLerpTime;

            //make the transition smooth af
            percentage = percentage * percentage * percentage * (percentage * (6f * percentage - 15f) + 10f);

            transform.position = Vector3.Lerp(startPos, endPos, percentage);
            //transform.rotation = Quaternion.Lerp(startRot, endRot, percentage);

            deltaTime += Time.deltaTime;

            yield return null;
        }
        moveAndRotate = null;
    }

    IEnumerator MoveAndRotate(Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot, GameObject goal)
    {
        StartCoroutine(MoveAndRotate(startPos, endPos, startRot, endRot, goal, lerpTime));
        yield return null;
    }
}
