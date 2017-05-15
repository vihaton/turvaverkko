using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleStatsScreenScript : MonoBehaviour {

    public GameObject StatsScreenLookPoint;
    public bool inStatsScreen = false;
    
    private LookPointMoveScript LPMS;
    private StatsPopupControlScript SPCS;

    private void Start()
    {
        LPMS = FindObjectOfType<LookPointMoveScript>();
        SPCS = FindObjectOfType<StatsPopupControlScript>();
    }

    public void StatsScreenToggle()
    {
        if (!inStatsScreen)
        {
            SetupStatsScreen(true);
        }
        else
        {
            SetupStatsScreen(false);
        }
        
        inStatsScreen = !inStatsScreen;
    }

    private void SetupStatsScreen(bool b)
    {
        if (b)
        {
            LPMS.MoveTo(StatsScreenLookPoint);
        } else
        {
            LPMS.MoveBackToPreviousPoint();
        }
        LPMS.LockPosition(b);
        SPCS.ShowStats(b);
    }
}
