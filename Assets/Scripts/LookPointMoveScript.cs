using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPointMoveScript : MonoBehaviour {

    public float lerpTime;
    public GameObject safetyNetArea;

    private GameObject previousLookPoint;
    private GameObject previousGoal;

    private void Start()
    {
        GameObject lookPoint;

        if (safetyNetArea.transform.childCount > 0)
        {
            lookPoint = safetyNetArea.transform.GetComponentsInChildren<Transform>()[1].gameObject;
        } else
        {
            lookPoint = safetyNetArea;
        }

        previousLookPoint = lookPoint;
        MoveTo(lookPoint);
    }

    public void MoveBackToPreviousPoint()
    {
        MoveTo(previousLookPoint);
    }

    public void MoveTo(GameObject goal)
    {
        previousLookPoint = previousGoal;
        previousGoal = goal;

        Vector3 startPos = transform.position;
        Vector3 endPos = goal.transform.position;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = goal.transform.rotation;

        Debug.Log("Moving to: (" + endPos.x + ", " + endPos.y + ", " + endPos.z + ")");

        StartCoroutine(MoveAndRotate(startPos, endPos, startRot, endRot, goal));
    }

    IEnumerator MoveAndRotate(Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot, GameObject goal)
    {
        float deltaTime = 0;
        while (deltaTime < lerpTime)
        {
            float percentage = deltaTime / lerpTime;

            //make the transition smooth af
            percentage = percentage * percentage * percentage * (percentage * (6f * percentage - 15f) + 10f);

            transform.position = Vector3.Lerp(startPos, endPos, percentage);
            //transform.rotation = Quaternion.Lerp(startRot, endRot, percentage);

            deltaTime += Time.deltaTime;

            yield return null;
        }
    }
}
