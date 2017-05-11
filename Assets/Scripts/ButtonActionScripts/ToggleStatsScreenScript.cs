using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleStatsScreenScript : MonoBehaviour {

    public GameObject StatsScreenLookPoint;
    public bool inStatsScreen = false;
    
    private LookPointMoveScript LPMS;

    private void Start()
    {
        LPMS = FindObjectOfType<LookPointMoveScript>();
    }

    public void StatsScreenToggle()
    {
        if (!inStatsScreen)
        {
            LPMS.MoveTo(StatsScreenLookPoint);
        }
        else
        {
            LPMS.MoveBackToPreviousPoint();
        }
        
        inStatsScreen = !inStatsScreen;
    }
}
