using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPointMoveScript : MonoBehaviour {

    public Vector3 endPos;
    public float lerpTime;

	public void Move()
    {
        Vector3 startPos = transform.position;
        Debug.Log("Moving to: (" + endPos.x + ", " + endPos.y + ", " + endPos.z + ")");
        StartCoroutine(MoveToPoint(startPos, endPos));
    }

    IEnumerator MoveToPoint(Vector3 startPos, Vector3 endPos)
    {
        float deltaTime = 0;
        while (deltaTime < lerpTime)
        {
            float percentage = deltaTime / lerpTime;

            //make the transition smooth af
            percentage = percentage * percentage * percentage * (percentage * (6f * percentage - 15f) + 10f);


            transform.position = Vector3.Lerp(startPos, endPos, percentage);

            deltaTime += Time.deltaTime;

            yield return null;
        }
    }
}
