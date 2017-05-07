using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeDetectionScript : MonoBehaviour {

    public GameObject testButton;
    public Text testButtonText;

    private float fingerStartTime = 0.0f;
    private Vector2 fingerStartPos = Vector2.zero;

    private bool isSwipe = false;
    private float minSwipeDist = 50.0f;
    private float maxSwipeTime = 0.5f;


    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        /* this is a new touch */
                        isSwipe = true;
                        fingerStartTime = Time.time;
                        fingerStartPos = touch.position;
                        break;

                    case TouchPhase.Canceled:
                        /* The touch is being canceled */
                        isSwipe = false;
                        break;

                    case TouchPhase.Ended:

                        float gestureTime = Time.time - fingerStartTime;
                        float gestureDist = (touch.position - fingerStartPos).magnitude;

                        if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist)
                        {
                            Vector2 direction = touch.position - fingerStartPos;
                            Vector2 swipeType = Vector2.zero;

                            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                            {
                                // the swipe is horizontal:
                                swipeType = Vector2.right * Mathf.Sign(direction.x);
                            }
                            else
                            {
                                // the swipe is vertical:
                                swipeType = Vector2.up * Mathf.Sign(direction.y);
                            }

                            if (swipeType.x != 0.0f)
                            {
                                if (swipeType.x > 0.0f)
                                {
                                    // MOVE RIGHT
                                    testButtonText.text = "Swiped right!";
                                    testButton.SetActive(true);
                                }
                                else
                                {
                                    // MOVE LEFT
                                    testButtonText.text = "Swiped left!";
                                    testButton.SetActive(true);
                                }
                            }

                            if (swipeType.y != 0.0f)
                            {
                                if (swipeType.y > 0.0f)
                                {
                                    // MOVE UP
                                    testButtonText.text = "Swiped up!";
                                    testButton.SetActive(true);
                                }
                                else
                                {
                                    // MOVE DOWN
                                    testButtonText.text = "Swiped down!";
                                    testButton.SetActive(true);
                                }
                            }

                        }

                        break;
                }
            }
        }

    }
}
