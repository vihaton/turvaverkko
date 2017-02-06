using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleStatsScreenScript : MonoBehaviour {

    public GameObject StatsScreenLookPoint;
    public GameObject SafetyNetLookPoint;

    private bool inStatsScreen = false;

	public void StatsScreenToggle()
    {
        GameObject temp = SafetyNetLookPoint;

        if (!inStatsScreen)
        {
            temp = StatsScreenLookPoint;
        }

        inStatsScreen = !inStatsScreen;

        FindObjectOfType<LookPointMoveScript>().Move(temp);
    }
}
