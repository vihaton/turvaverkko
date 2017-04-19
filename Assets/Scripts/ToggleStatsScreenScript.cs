using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleStatsScreenScript : MonoBehaviour {

    public GameObject StatsScreenLookPoint;
    public GameObject SafetyNetLookPoint;

    private LookPointMoveScript LPMS;
    private bool inStatsScreen = false;

    private void Start()
    {
        LPMS = FindObjectOfType<LookPointMoveScript>();
    }

    public void StatsScreenToggle()
    {
        inStatsScreen = !inStatsScreen;

        if (!inStatsScreen)
        {
            LPMS.Move(StatsScreenLookPoint);
        } else
        {
            LPMS.Move(SafetyNetLookPoint);
        }
    }
}
